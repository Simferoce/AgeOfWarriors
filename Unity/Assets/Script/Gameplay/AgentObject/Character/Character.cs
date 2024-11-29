using Assets.Script.Agent.Technology;
using Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Attackable))]
    [RequireComponent(typeof(AttackFactory))]
    [RequireComponent(typeof(Target))]
    public partial class Character : AgentObject<CharacterDefinition>, IModifierSource, IBlock, IAnimated
    {
        [Header("Collision")]
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private Collider2D hitbox;

        [Header("Target")]
        [SerializeField] private Transform targetPosition;

        public event Action OnDeath;
        public event Action<Modifier> OnModifierAdded;

        public Animated Animated { get; set; }
        public List<Modifier> AppliedModifiers { get; set; } = new List<Modifier>();
        public override bool IsActive { get => !IsDead; }
        public Vector3 CenterPosition { get => transform.position; }
        public Vector3 TargetPosition => targetPosition.position;
        public Collider2D Hitbox { get => hitbox; set => hitbox = value; }
        public override Faction Faction => IsConfused ? Agent.Faction.GetConfusedFaction() : Agent.Faction;

        public float Health { get; set; }
        public float MaxHealth { get => Definition.MaxHealth + GetCachedComponent<ModifierHandler>().GetModifiers().Aggregate(0f, (a, b) => b.StatisticRegistry.TryGetStatistic<float>(StatisticDefinition.FlatMaxHealth, out Statistic<float> value) ? a + value.GetValue() : a); }
        public float Defense { get => Definition.Defense + GetCachedComponent<ModifierHandler>().GetModifiers().Aggregate(0f, (a, b) => b.StatisticRegistry.TryGetStatistic<float>(StatisticDefinition.FlatDefense, out Statistic<float> value) ? a + value.GetValue() : a); }
        public float AttackSpeed { get => Definition.AttackSpeed * (1 + GetCachedComponent<ModifierHandler>().GetModifiers().Aggregate(0f, (a, b) => b.StatisticRegistry.TryGetStatistic<float>(StatisticDefinition.PercentageAttackSpeed, out Statistic<float> value) ? a + value.GetValue() : a)); }
        public float AttackPower { get => Definition.AttackPower + GetCachedComponent<ModifierHandler>().GetModifiers().Aggregate(0f, (a, b) => b.StatisticRegistry.TryGetStatistic<float>(StatisticDefinition.FlatAttackPower, out Statistic<float> value) ? a + value.GetValue() : a); }
        public float Speed { get => Definition.Speed * (1 + GetCachedComponent<ModifierHandler>().GetModifiers().Aggregate(0f, (a, b) => b.StatisticRegistry.TryGetStatistic<float>(StatisticDefinition.PercentageSpeed, out Statistic<float> value) ? a + value.GetValue() : a)); }
        public float Reach { get => Definition.Reach * (1 + GetCachedComponent<ModifierHandler>().GetModifiers().Aggregate(0f, (a, b) => b.StatisticRegistry.TryGetStatistic<float>(StatisticDefinition.PercentageReach, out Statistic<float> value) ? a + value.GetValue() : a)); }
        public float TechnologyPerSecond => Definition.TechnologyPerSecond;

        public bool IsEngaged => TargetUtility.GetTargets((x) => Mathf.Abs(x.CenterPosition.x - this.CenterPosition.x) < 0.5f).Count > 0;
        public bool IsInvulnerable => GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.IsInvulnerable.HasValue).Any(x => x.IsInvulnerable.Value);
        public bool IsConfused => GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.IsConfused.HasValue).Any(x => x.IsConfused.Value);
        public bool IsDead { get => stateMachine.Current is DeathState; }
        public bool IsInjured { get => Health < MaxHealth; }

        private Statistic<float> health;
        private Statistic<float> maxHealth;
        private Statistic<float> defense;
        private Statistic<float> attackSpeed;
        private Statistic<float> attackPower;
        private Statistic<float> speed;
        private Statistic<float> reach;
        private Statistic<float> technologyPerSeconds;

        private StateMachine stateMachine = new StateMachine();

        protected override void Awake()
        {
            base.Awake();
            GetCachedComponent<Attackable>().OnAttackTaken += OnAttackTaken;
            GetCachedComponent<AttackFactory>().OnAttackDealt += OnAttackDealt;
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
                    this.GetCachedComponent<ModifierHandler>().AddModifier(modifier.GetModifier(this.GetCachedComponent<ModifierHandler>()));
            }

            health = new Statistic<float>(StatisticDefinition.Health);
            maxHealth = new Statistic<float>(StatisticDefinition.MaxHealth);
            defense = new Statistic<float>(StatisticDefinition.Defense);
            attackSpeed = new Statistic<float>(StatisticDefinition.AttackSpeed);
            attackPower = new Statistic<float>(StatisticDefinition.AttackPower);
            speed = new Statistic<float>(StatisticDefinition.Speed);
            reach = new Statistic<float>(StatisticDefinition.Reach);
            technologyPerSeconds = new Statistic<float>(StatisticDefinition.TechnologyPerSeconds);

            StatisticRegistry.Register(health);
            StatisticRegistry.Register(maxHealth);
            StatisticRegistry.Register(defense);
            StatisticRegistry.Register(attackSpeed);
            StatisticRegistry.Register(attackPower);
            StatisticRegistry.Register(speed);
            StatisticRegistry.Register(reach);
            StatisticRegistry.Register(technologyPerSeconds);

            stateMachine.Initialize(new MoveState(this));
            this.Health = MaxHealth;
        }

        private void Update()
        {
            if (IsDead)
                return;

            stateMachine.Update();

            health.SetValue(Health);
            maxHealth.SetValue(MaxHealth);
            defense.SetValue(Defense);
            attackSpeed.SetValue(AttackSpeed);
            attackPower.SetValue(AttackPower);
            speed.SetValue(Speed);
            reach.SetValue(Reach);
            technologyPerSeconds.SetValue(TechnologyPerSecond);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetCachedComponent<Attackable>().OnAttackTaken -= OnAttackTaken;
            GetCachedComponent<AttackFactory>().OnAttackDealt -= OnAttackDealt;
        }

        private void OnAttackTaken(AttackResult attackResult)
        {
            Health -= attackResult.DamageTaken;
            if (Health < 0)
                Death();
        }

        private void OnAttackDealt(AttackResult result)
        {
            Heal(result.DamageTaken * result.Attack.Leach);
        }

        public void SetDirection()
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

        public Vector3 ClosestPoint(Vector3 point)
        {
            return Hitbox.ClosestPoint(point);
        }

        public bool IsBlocking(Character character)
        {
            return character.Hitbox.IsTouching(hitbox)
                && (character.OriginalFaction != this.OriginalFaction
                || character.Priority > this.Priority);
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
            this.Health += amount;
            this.Health = Mathf.Clamp(Health, 0, MaxHealth);
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