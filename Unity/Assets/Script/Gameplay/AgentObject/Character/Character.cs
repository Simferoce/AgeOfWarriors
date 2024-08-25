using Assets.Script.Agent.Technology;
using Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public partial class Character : AgentObject<CharacterDefinition>, IAttackSource, IAttackable, ITargeteable, IModifierSource, IBlock, IAnimated
    {
        [Header("Collision")]
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private Collider2D hitbox;

        [Header("Target")]
        [SerializeField] private Transform targetPosition;

        public event Action OnDeath;
        public event AttackedLanded OnAttackLanded;
        public event Action<Modifier> OnModifierAdded;
        public event Action<AttackResult, IAttackable> OnDamageTaken;

        public Animated Animated { get; set; }
        public List<TransformTag> TransformTags { get; set; }
        public List<Modifier> AppliedModifiers { get; set; } = new List<Modifier>();
        public HashSet<IAttackable> RecentlyAttackedAttackeables { get; set; } = new HashSet<IAttackable>();
        public override bool IsActive { get => !IsDead; }
        public Vector3 CenterPosition { get => transform.position; }
        public Vector3 TargetPosition => targetPosition.position;
        public Collider2D Hitbox { get => hitbox; set => hitbox = value; }
        public override Faction Faction => IsConfused ? Agent.Faction.GetConfusedFaction() : Agent.Faction;
        public override string StatisticProviderName => "character";

        public float Health { get; set; }
        public float MaxHealth { get => Definition.MaxHealth + GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.MaxHealth.HasValue).Sum(x => x.MaxHealth.Value); }
        public float Defense { get => Definition.Defense + GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.Defense.HasValue).Sum(x => x.Defense.Value); }
        public float AttackSpeed { get => Definition.AttackSpeed * (1 + GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.AttackSpeedPercentage.HasValue).Sum(x => x.AttackSpeedPercentage.Value)); }
        public float AttackPower { get => Definition.AttackPower + GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.AttackPower.HasValue).Sum(x => x.AttackPower.Value); }
        public float Speed { get => Definition.Speed * (1 + GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.SpeedPercentage.HasValue).Sum(x => x.SpeedPercentage.Value)); }
        public float Reach { get => Definition.Reach * (1 + GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.ReachPercentage.HasValue).Sum(x => x.ReachPercentage.Value)); }
        public float TechnologyGainPerSecond => Definition.TechnologyGainPerSecond;

        public bool IsEngaged => TargetUtility.GetTargets(this, engagedCriteria, this).FirstOrDefault() != null;
        public bool IsInvulnerable => GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.IsInvulnerable.HasValue).Any(x => x.IsInvulnerable.Value);
        public bool IsConfused => GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.IsConfused.HasValue).Any(x => x.IsConfused.Value);
        public bool IsDead { get => stateMachine.Current is DeathState; }
        public bool IsInjured { get => Health < MaxHealth; }

        private StateMachine stateMachine = new StateMachine();
        private TargetCriteria engagedCriteria = new IsEnemyTargetCriteria();

        protected override void Awake()
        {
            base.Awake();
            TransformTags = GetComponentsInChildren<TransformTag>().ToList();
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
                    this.GetCachedComponent<IModifiable>().AddModifier(modifier.GetModifier(this.GetCachedComponent<IModifiable>()));
            }

            stateMachine.Initialize(new MoveState(this));
            this.Health = MaxHealth;
        }

        public void Update()
        {
            RecentlyAttackedAttackeables.RemoveWhere(x => x is UnityEngine.Object o && o == null);
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

            GetCachedComponent<IModifiable>().Clear();
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

        public void AttackLanded(AttackResult attackResult)
        {
            RecentlyAttackedAttackeables.Add(attackResult.Target);
            Heal(attackResult.DamageTaken * attackResult.Attack.Leach);
            OnAttackLanded?.Invoke(attackResult);
        }

        public bool RecentlyAttacked(IAttackable attackable)
        {
            return RecentlyAttackedAttackeables.Contains(attackable);
        }

        public void TakeAttack(Attack attack)
        {
            AttackHandler.Result result = AttackHandler.TakeAttack(attack, new AttackHandler.Input(
                    this,
                    currentHealth: Health,
                    defense: Defense,
                    increaseDamageTaken: GetCachedComponent<IModifiable>().GetModifiers().Sum(x => x.IncreaseDamageTaken ?? 0),
                    rangedDamageReduction: GetCachedComponent<IModifiable>().GetModifiers().Sum(x => x.RangedDamageReduction ?? 0),
                    shields: GetCachedComponent<IModifiable>().GetModifiers().OfType<ShieldModifierDefinition.Shield>().ToList(),
                    canResistDeath: GetCachedComponent<IModifiable>().GetModifiers().Any(x => x is ResistKillingBlowPerk.Modifier modifier && modifier.CanResistsKillingBlow())));

            Health -= result.DamageToTake;

            if (result.ResistedDeath)
            {
                ResistKillingBlowPerk.Modifier modifier = (ResistKillingBlowPerk.Modifier)GetCachedComponent<IModifiable>().GetModifiers().FirstOrDefault(x => x is ResistKillingBlowPerk.Modifier modifier && modifier.CanResistsKillingBlow());
                if (modifier != null)
                {
                    modifier.ResistKillingBlow();
                    Health = 0.001f;
                }
            }

            AttackResult attackResult = new AttackResult(attack, result.DamageToTake, result.DefenseDamagePrevented, Health <= 0, this);
            foreach (IAttackSource source in attack.AttackSource.Sources)
                source.AttackLanded(attackResult);

            OnDamageTaken?.Invoke(attackResult, this);

            if (Health <= 0 && !IsDead)
                Death();
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