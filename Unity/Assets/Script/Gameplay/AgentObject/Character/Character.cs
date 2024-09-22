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

        public float Health { get; set; }
        public float MaxHealth => this.Definition.MaxHealth;
        public float Defense => this.Definition.Defense + this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.Defense.HasValue).Sum(x => x.Defense.Value);
        public float AttackSpeed => this.Definition.AttackSpeed * (1 + this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.AttackSpeedPercentage.HasValue).Sum(x => x.AttackSpeedPercentage.Value));
        public float AttackPower => this.Definition.AttackPower + this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.AttackPower.HasValue).Sum(x => x.AttackPower.Value);
        public float Speed => this.Definition.Speed * (1 + this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.SpeedPercentage.HasValue).Sum(x => x.SpeedPercentage.Value));
        public float Reach => this.Definition.Reach * (1 + this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.ReachPercentage.HasValue).Sum(x => x.ReachPercentage.Value));
        public float TechnologyGainPerSecond => Definition.TechnologyGainPerSecond;

        public bool IsEngaged => TargetUtility.GetTargets(this, this.engagedCriteria, this).FirstOrDefault() != null;
        public bool IsInvulnerable => this.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is IInvulnerableModifier);
        public bool IsConfused => this.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is ConfusionModifierDefinition.Modifier);
        public bool IsDead => this.stateMachine.Current is DeathState;
        public bool IsInjured => this.Health < this.MaxHealth;

        private StateMachine stateMachine = new StateMachine();
        private TargetCriteria engagedCriteria = new IsEnemyTargetCriteria();

        public override IEnumerable<Statistic> GetStatistic()
        {
            yield return new StatisticTemporary<float>(this, "health", Health, StatisticRepository.GetDefinition(StatisticRepository.Health));
            yield return new StatisticTemporary<float>(this, "maxhealth", MaxHealth, StatisticRepository.GetDefinition(StatisticRepository.MaxHealth));
            yield return new StatisticTemporary<float>(this, "defense", Defense, StatisticRepository.GetDefinition(StatisticRepository.Defense));
            yield return new StatisticTemporary<float>(this, "attack_speed", AttackSpeed, StatisticRepository.GetDefinition(StatisticRepository.AttackSpeed));
            yield return new StatisticTemporary<float>(this, "attack_power", AttackPower, StatisticRepository.GetDefinition(StatisticRepository.AttackPower));
            yield return new StatisticTemporary<float>(this, "speed", Speed, StatisticRepository.GetDefinition(StatisticRepository.Speed));
            yield return new StatisticTemporary<float>(this, "reach", Reach, StatisticRepository.GetDefinition(StatisticRepository.Reach));
            yield return new StatisticTemporary<bool>(this, "engaged", IsEngaged);
            yield return new StatisticTemporary<bool>(this, "invulnerable", IsInvulnerable);
            yield return new StatisticTemporary<bool>(this, "confused", IsConfused);
            yield return new StatisticTemporary<bool>(this, "isDead", IsDead);
            yield return new StatisticTemporary<bool>(this, "isInjured", IsInjured);

            foreach (Statistic statistic in base.GetStatistic())
                yield return statistic;
        }

        protected override void Awake()
        {
            base.Awake();
            TransformTags = GetComponentsInChildren<TransformTag>().ToList();
            GetCachedComponent<Attackable>().OnDamageTaken += Character_OnDamageTaken;
        }

        private void Character_OnDamageTaken(AttackResult attackResult, Attackable attackable)
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
            List<ITechnologyModify> modifiers = agent.Technology.UnlockedPerks().OfType<ITechnologyModify>().ToList();
            foreach (ITechnologyModify modifier in modifiers)
            {
                if (modifier.Affect(agentObjectDefinition))
                    this.Entity.GetCachedComponent<ModifierHandler>().AddModifier(modifier.GetModifier(this.Entity.GetCachedComponent<ModifierHandler>()));
            }

            stateMachine.Initialize(new MoveState(this));
            Health = MaxHealth;
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
            Health += amount;
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