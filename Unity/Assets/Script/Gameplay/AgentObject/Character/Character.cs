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
    public partial class Character : AgentObject<CharacterDefinition>, ITargeteable, IModifierSource, IBlock, IAnimated
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
        public float MaxHealth { get => Definition.MaxHealth + GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.MaxHealth.HasValue).Sum(x => x.MaxHealth.Value); }
        public float Defense { get => Definition.Defense + GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.Defense.HasValue).Sum(x => x.Defense.Value); }
        public float AttackSpeed { get => Definition.AttackSpeed * (1 + GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.AttackSpeedPercentage.HasValue).Sum(x => x.AttackSpeedPercentage.Value)); }
        public float AttackPower { get => Definition.AttackPower + GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.AttackPower.HasValue).Sum(x => x.AttackPower.Value); }
        public float Speed { get => Definition.Speed * (1 + GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.SpeedPercentage.HasValue).Sum(x => x.SpeedPercentage.Value)); }
        public float Reach { get => Definition.Reach * (1 + GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.ReachPercentage.HasValue).Sum(x => x.ReachPercentage.Value)); }
        public float TechnologyGainPerSecond => Definition.TechnologyGainPerSecond;

        public bool IsEngaged => TargetUtility.GetTargets(this, engagedCriteria, this).FirstOrDefault() != null;
        public bool IsInvulnerable => GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.IsInvulnerable.HasValue).Any(x => x.IsInvulnerable.Value);
        public bool IsConfused => GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.IsConfused.HasValue).Any(x => x.IsConfused.Value);
        public bool IsDead { get => stateMachine.Current is DeathState; }
        public bool IsInjured { get => Health < MaxHealth; }

        private StateMachine stateMachine = new StateMachine();
        private TargetCriteria engagedCriteria = new IsEnemyTargetCriteria();

        protected override void Awake()
        {
            base.Awake();
            GetCachedComponent<Attackable>().OnAttackTaken += OnAttackTaken;
            GetCachedComponent<AttackFactory>().OnAttackDealt += OnAttackDealt;
        }

        public override bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic)
        {
            if (path.SequenceEqual("isDead"))
            {
                statistic = StatisticUtility.ConvertGeneric<T, bool>(IsDead);
                return true;
            }
            else if (path.SequenceEqual("health"))
            {
                statistic = StatisticUtility.ConvertGeneric<T, float>(Health);
                return true;
            }
            else if (path.SequenceEqual("maxhealth"))
            {
                statistic = StatisticUtility.ConvertGeneric<T, float>(MaxHealth);
                return true;
            }
            else if (path.SequenceEqual("defense"))
            {
                statistic = StatisticUtility.ConvertGeneric<T, float>(Defense);
                return true;
            }
            else if (path.SequenceEqual("attackspeed"))
            {
                statistic = StatisticUtility.ConvertGeneric<T, float>(AttackSpeed);
                return true;
            }
            else if (path.SequenceEqual("attackpower"))
            {
                statistic = StatisticUtility.ConvertGeneric<T, float>(AttackPower);
                return true;
            }
            else if (path.SequenceEqual("speed"))
            {
                statistic = StatisticUtility.ConvertGeneric<T, float>(Speed);
                return true;
            }
            else if (path.SequenceEqual("reach"))
            {
                statistic = StatisticUtility.ConvertGeneric<T, float>(Reach);
                return true;
            }

            return base.TryGetStatistic<T>(name, out statistic);
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

            stateMachine.Initialize(new MoveState(this));
            this.Health = MaxHealth;
        }

        public void FixedUpdate()
        {
            if (IsDead)
                return;

            stateMachine.Update();
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