using Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Blocker))]
    [RequireComponent(typeof(Attackable))]
    [RequireComponent(typeof(Target))]
    [RequireComponent(typeof(AttackFactory))]
    [RequireComponent(typeof(ModifierApplier))]
    public partial class Character : AgentObject<CharacterDefinition>
    {
        [Header("Collision")]
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private Collider2D hitbox;

        public event Action OnDeath;

        public Animated Animated { get; set; }
        public List<TransformTag> TransformTags { get; set; }
        public override bool IsActive { get => !IsDead; }
        public Collider2D Hitbox { get => hitbox; set => hitbox = value; }
        public override Faction Faction => IsConfused ? Agent.Faction.GetConfusedFaction() : Agent.Faction;

        public float Health { get => health; set => health.Modify(value); }
        public float MaxHealth => health.Max;
        public float Defense => defense /*+ this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.Defense.HasValue).Sum(x => x.Defense.Value)*/;
        public float AttackSpeed => attackSpeed /** (1 + this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.AttackSpeedPercentage.HasValue).Sum(x => x.AttackSpeedPercentage.Value))*/;
        public float AttackPower => attackPower /*+ this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.AttackPower.HasValue).Sum(x => x.AttackPower.Value)*/;
        public float Speed => speed/* * (1 + this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.SpeedPercentage.HasValue).Sum(x => x.SpeedPercentage.Value))*/;
        public float Reach => reach /** (1 + this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.ReachPercentage.HasValue).Sum(x => x.ReachPercentage.Value))*/;
        public float TechnologyGainPerSecond => Definition.TechnologyGainPerSecond;

        public bool IsEngaged => TargetUtility.GetTargets(this, this.engagedCriteria, this).FirstOrDefault() != null;
        public bool IsInvulnerable => this.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is IInvulnerableModifier);
        public bool IsConfused => false;/*this.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is ConfusionModifierDefinition.Modifier);*/
        public bool IsStaggered => false;/*this.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is StaggerModifierDefinition.Modifier);*/
        public bool IsDead => this.stateMachine.Current is DeathState;
        public bool IsInjured => this.Health < this.MaxHealth;

        private StateMachine stateMachine = new StateMachine();
        private TargetCriteria engagedCriteria = new IsEnemyTargetCriteria();

        private StatisticFloatModifiable health;
        private StatisticSerialize<float> defense;
        private StatisticSerialize<float> attackPower;
        private StatisticSerialize<float> attackSpeed;
        private StatisticSerialize<float> speed;
        private StatisticSerialize<float> reach;

        protected override void Awake()
        {
            base.Awake();
            TransformTags = GetComponentsInChildren<TransformTag>().ToList();
            GetCachedComponent<Attackable>().OnDamageTaken += OnDamageTaken;
            GetCachedComponent<AttackFactory>().OnAttackLanded += OnAttackLanded;
        }

        public override IEnumerable<Statistic> GetStatistic()
        {
            yield return health;
            yield return defense;
            yield return attackPower;
            yield return attackSpeed;
            yield return speed;
            yield return reach;

            foreach (Statistic statistic in base.GetStatistic())
                yield return statistic;
        }


        private void OnDamageTaken(AttackResult attackResult, Attackable attackable)
        {
            Health -= attackResult.DamageTaken;
            if (Health <= 0 && !IsDead)
                Death();
        }

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            Animated = GetComponentInChildren<Animated>();

            AgentObjectDefinition agentObjectDefinition = GetDefinition();
            List<CharacterTechnologyPerkDefinition> modifiers = agent.Technology.UnlockedPerks().OfType<CharacterTechnologyPerkDefinition>().ToList();
            foreach (CharacterTechnologyPerkDefinition modifier in modifiers)
            {
                if (modifier.Affect(agentObjectDefinition))
                    modifier.Modify(agent, this);
            }

            health = new StatisticFloatModifiable("health", StatisticRepository.Health, new StatisticSerialize<float>("max", StatisticRepository.MaxHealth, this.Definition.MaxHealth));
            defense = new StatisticSerialize<float>("defense", StatisticRepository.Defense, Definition.Defense);
            attackPower = new StatisticSerialize<float>("attackPower", StatisticRepository.AttackPower, Definition.AttackPower);
            attackSpeed = new StatisticSerialize<float>("attackSpeed", StatisticRepository.AttackSpeed, Definition.AttackSpeed);
            speed = new StatisticSerialize<float>("speed", StatisticRepository.Speed, Definition.Speed);
            reach = new StatisticSerialize<float>("reach", StatisticRepository.Reach, Definition.Reach);

            health.Initialize(this);
            defense.Initialize(this);
            attackPower.Initialize(this);
            attackSpeed.Initialize(this);
            speed.Initialize(this);
            reach.Initialize(this);

            Health = MaxHealth;

            stateMachine.Initialize(new MoveState(this));
        }

        public void FixedUpdate()
        {
            if (IsDead)
                return;

            stateMachine.FixedUpdate();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetCachedComponent<Attackable>().OnDamageTaken -= OnDamageTaken;
            GetCachedComponent<AttackFactory>().OnAttackLanded -= OnAttackLanded;
        }

        public void RefreshDirection()
        {
            transform.localScale = new Vector3(Mathf.Sign(IsConfused ? -Direction : Direction) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        public void Death()
        {
            EventChannelDeath.Instance.Publish(new EventChannelDeath.Event() { AgentObject = this });
            OnDeath?.Invoke();
            stateMachine.SetState(new DeathState(this));
        }

        public void Displace(Vector2 displacement)
        {
            rigidbody.MovePosition(this.rigidbody.position + displacement);
        }

        public void Heal(float amount)
        {
            Health += amount;
        }

        public void OnAttackLanded(AttackResult attackResult)
        {
            Heal(attackResult.DamageTaken * attackResult.Attack.Leach);
        }

        public void BeginCast()
        {
            stateMachine.SetState(new CastingState(this));
        }

        public void EndCast()
        {
            if (stateMachine.Next == null)
                stateMachine.SetState(new MoveState(this));
        }
    }
}