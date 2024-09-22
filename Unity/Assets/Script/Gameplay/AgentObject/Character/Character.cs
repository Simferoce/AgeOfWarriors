using Assets.Script.Agent.Technology;
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
    public partial class Character : AgentObject<CharacterDefinition>, IAttackSource, IModifierSource
    {
        [Header("Collision")]
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private Collider2D hitbox;

        public event Action OnDeath;
        public event AttackedLanded OnAttackLanded;
        public event Action<Modifier> OnModifierAdded;
        public event Action<AttackResult, Attackable> OnDamageTaken;

        public Animated Animated { get; set; }
        public List<TransformTag> TransformTags { get; set; }
        public List<Modifier> AppliedModifiers { get; set; } = new List<Modifier>();
        public HashSet<Attackable> RecentlyAttackedAttackeables { get; set; } = new HashSet<Attackable>();
        public override bool IsActive { get => !IsDead; }
        public Collider2D Hitbox { get => hitbox; set => hitbox = value; }
        public override Faction Faction => IsConfused ? Agent.Faction.GetConfusedFaction() : Agent.Faction;
        public Entity Entity { get; set; }

        public float Health => health;
        public float MaxHealth => health.Max;
        public float Defense => defense;
        public float AttackSpeed => attackSpeed;
        public float AttackPower => attackPower;
        public float Speed => speed;
        public float Reach => reach;

        public bool IsEngaged => isEngaged;
        public bool IsInvulnerable => isInvulnerable;
        public bool IsConfused => isConfused;
        public bool IsDead => isDead;
        public bool IsInjured => isInjured;

        private StatisticFloatModifiable health = new StatisticFloatModifiable("health", StatisticRepository.Health, new StatisticFunction<Character, float>("max", StatisticRepository.MaxHealth, x => x.Definition.MaxHealth));
        private StatisticFunction<Character, float> defense = new StatisticFunction<Character, float>("defense", StatisticRepository.Defense, x => x.Definition.Defense + x.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.Defense.HasValue).Sum(x => x.Defense.Value));
        private StatisticFunction<Character, float> attackSpeed = new StatisticFunction<Character, float>("attack_speed", StatisticRepository.AttackSpeed, x => x.Definition.AttackSpeed * (1 + x.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.AttackSpeedPercentage.HasValue).Sum(x => x.AttackSpeedPercentage.Value)));
        private StatisticFunction<Character, float> attackPower = new StatisticFunction<Character, float>("attack_power", StatisticRepository.AttackPower, x => x.Definition.AttackPower + x.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.AttackPower.HasValue).Sum(x => x.AttackPower.Value));
        private StatisticFunction<Character, float> speed = new StatisticFunction<Character, float>("speed", StatisticRepository.Speed, x => x.Definition.Speed * (1 + x.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.SpeedPercentage.HasValue).Sum(x => x.SpeedPercentage.Value)));
        private StatisticFunction<Character, float> reach = new StatisticFunction<Character, float>("reach", StatisticRepository.Reach, x => x.Definition.Reach * (1 + x.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.ReachPercentage.HasValue).Sum(x => x.ReachPercentage.Value)));
        public float TechnologyGainPerSecond => Definition.TechnologyGainPerSecond;

        private StatisticFunction<Character, bool> isEngaged = new StatisticFunction<Character, bool>("engaged", null, x => TargetUtility.GetTargets(x, x.engagedCriteria, x).FirstOrDefault() != null);
        private StatisticFunction<Character, bool> isInvulnerable = new StatisticFunction<Character, bool>("invulnerable", null, x => x.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.IsInvulnerable.HasValue).Any(x => x.IsInvulnerable.Value));
        private StatisticFunction<Character, bool> isConfused = new StatisticFunction<Character, bool>("confused", null, x => x.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.IsConfused.HasValue).Any(x => x.IsConfused.Value));
        private StatisticFunction<Character, bool> isDead = new StatisticFunction<Character, bool>("dead", null, x => x.stateMachine.Current is DeathState);
        private StatisticFunction<Character, bool> isInjured = new StatisticFunction<Character, bool>("injured", null, x => x.Health < x.MaxHealth);

        private StateMachine stateMachine = new StateMachine();
        private TargetCriteria engagedCriteria = new IsEnemyTargetCriteria();

        public override Statistic GetStatistic(ReadOnlySpan<char> value)
        {
            if (value.SequenceEqual(health.Name))
                return health;

            if (value.SequenceEqual(defense.Name))
                return defense;

            if (value.SequenceEqual(attackSpeed.Name))
                return attackSpeed;

            if (value.SequenceEqual(attackPower.Name))
                return attackPower;

            if (value.SequenceEqual(speed.Name))
                return speed;

            if (value.SequenceEqual(reach.Name))
                return reach;

            if (value.SequenceEqual(isEngaged.Name))
                return isEngaged;

            if (value.SequenceEqual(isInvulnerable.Name))
                return isInvulnerable;

            if (value.SequenceEqual(isConfused.Name))
                return isConfused;

            if (value.SequenceEqual(isDead.Name))
                return isDead;

            if (value.SequenceEqual(isInjured.Name))
                return isInjured;

            return base.GetStatistic(value);
        }

        public override IStatisticContext GetContext(ReadOnlySpan<char> value)
        {
            if (value.SequenceEqual("health"))
                return health;

            return base.GetContext(value);
        }

        protected override void Awake()
        {
            base.Awake();
            TransformTags = GetComponentsInChildren<TransformTag>().ToList();
            GetCachedComponent<Attackable>().OnDamageTaken += Character_OnDamageTaken;
        }

        private void Character_OnDamageTaken(AttackResult attackResult, Attackable attackable)
        {
            health.Modify(Health - attackResult.DamageTaken);
            if (health <= 0 && !IsDead)
                Death();
        }

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            Animated = GetComponentInChildren<Animated>();

            AgentObjectDefinition agentObjectDefinition = GetDefinition();
            List<ITechnologyModify> modifiers = agent.Technology.UnlockedPerks().OfType<ITechnologyModify>().ToList();
            foreach (ITechnologyModify modifier in modifiers)
            {
                if (modifier.Affect(agentObjectDefinition))
                    this.Entity.GetCachedComponent<ModifierHandler>().AddModifier(modifier.GetModifier(this.Entity.GetCachedComponent<ModifierHandler>()));
            }

            stateMachine.Initialize(new MoveState(this));
            this.health.Modify(MaxHealth);
        }

        public void Update()
        {
            RecentlyAttackedAttackeables.RemoveWhere(x => x is UnityEngine.Object o && o == null);
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
            GetCachedComponent<Attackable>().OnDamageTaken -= Character_OnDamageTaken;
        }

        public void RefreshDirection()
        {
            transform.localScale = new Vector3(Mathf.Sign(IsConfused ? -Direction : Direction) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        public void AddAppliedModifier(Modifier modifier)
        {
            AppliedModifiers.Add(modifier);
            OnModifierAdded?.Invoke(modifier);
        }

        public void RemoveAppliedModifier(Modifier modifier)
        {
            AppliedModifiers.Remove(modifier);
        }

        public void Death()
        {
            EventChannelDeath.Instance.Publish(new EventChannelDeath.Event() { AgentObject = this });
            OnDeath?.Invoke();
            stateMachine.SetState(new DeathState(this));

            GetCachedComponent<ModifierHandler>().Clear();
        }

        public void Displace(Vector2 displacement)
        {
            rigidbody.MovePosition(this.rigidbody.position + displacement);
        }

        public void Heal(float amount)
        {
            this.health.Modify(Health + amount);
        }

        public void AttackLanded(AttackResult attackResult)
        {
            RecentlyAttackedAttackeables.Add(attackResult.Target);
            Heal(attackResult.DamageTaken * attackResult.Attack.Leach);
            OnAttackLanded?.Invoke(attackResult);
        }

        public bool RecentlyAttacked(Attackable attackable)
        {
            return RecentlyAttackedAttackeables.Contains(attackable);
        }

        #region Ability
        public void BeginCast()
        {
            stateMachine.SetState(new CastingState(this));
        }

        public void EndCast()
        {
            if (stateMachine.Next == null)
                stateMachine.SetState(new MoveState(this));
        }
        #endregion
    }
}