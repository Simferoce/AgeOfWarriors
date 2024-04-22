using Assets.Script.Agent.Technology;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Attackable))]
    [RequireComponent(typeof(Target))]
    public partial class Character : AgentObject<CharacterDefinition>, IDisplaceable, IStaggerable, IAttackSource, IAttackableOwner, ITargetOwner
    {
        [Header("Collision")]
        [SerializeField] private new Rigidbody2D rigidbody;

        public CharacterAnimator CharacterAnimator { get; set; }
        public List<TransformTag> TransformTags { get; set; }
        public event AttackedLanded OnAttackLanded;
        public override bool IsActive { get => !IsDead; }
        public Vector3 CenterPosition { get => this.GetCachedComponent<ITargeteable>().CenterPosition; }

        public float Health { get; set; }
        public float MaxHealth { get => Definition.MaxHealth + GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.MaxHealth.HasValue).Sum(x => x.MaxHealth.Value); }
        public float Defense { get => Definition.Defense + GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.Defense.HasValue).Sum(x => x.Defense.Value); }
        public float AttackSpeed { get => Definition.AttackSpeed; }
        public float AttackPower { get => Definition.AttackPower + GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.AttackPower.HasValue).Sum(x => x.AttackPower.Value); }
        public float Speed { get => Definition.Speed * (1 + GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.SpeedPercentage.HasValue).Sum(x => x.SpeedPercentage.Value)); }
        public float Reach { get => Definition.Reach; }
        public float TechnologyGainPerSecond => Definition.TechnologyGainPerSecond;

        public bool IsEngaged => GetTarget(engagedCriteria, this) != null;
        public bool IsInvulnerable => GetCachedComponent<IModifiable>().GetModifiers().Where(x => x.Invulnerable.HasValue).Any(x => x.Invulnerable.Value);
        public bool IsDead { get => stateMachine.Current is DeathState; }
        public bool IsInjured { get => Health < MaxHealth; }

        private StateMachine stateMachine = new StateMachine();
        private TargetCriteria engagedCriteria = new IsEnemyTargetCriteria();

        protected override void Awake()
        {
            base.Awake();
            TransformTags = GetComponentsInChildren<TransformTag>().ToList();
        }

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            CharacterAnimator = GetComponentInChildren<CharacterAnimator>();

            AgentObjectDefinition agentObjectDefinition = GetDefinition();
            List<ITechnologyModify> modifiers = agent.Technology.PerksUnlocked.OfType<ITechnologyModify>().ToList();
            foreach (ITechnologyModify modifier in modifiers)
            {
                if (modifier.Affect(agentObjectDefinition))
                    this.GetCachedComponent<IModifiable>().AddModifier(modifier.GetModifier(this.GetCachedComponent<IModifiable>()));
            }

            stateMachine.Initialize(new MoveState(this));
            InitializeAbilities();
            this.Health = MaxHealth;
        }

        public void FixedUpdate()
        {
            if (IsDead)
                return;

            UpdateAbilities();

            stateMachine.Update();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            DisposeAbilities();
        }

        public Attack GenerateAttack(float damage, float armorPenetration, float leach, bool ranged, IAttackable target, params IAttackSource[] source)
        {
            bool empowered = false;

            List<Modifier> modifiers = GetCachedComponent<IModifiable>().GetModifiers();

            EmpoweredModifierDefinition.Modifier empowerment = modifiers.FirstOrDefault(x => x is EmpoweredModifierDefinition.Modifier) as EmpoweredModifierDefinition.Modifier;
            if (empowerment != null)
            {
                damage *= 1 + empowerment.PercentageDamageIncrease;
                empowerment.Consume();

                empowered = true;
            }

            if (modifiers.Count > 0)
            {
                float damageDealtReduction = modifiers.Max(x => x.DamageDealtReduction ?? 0);
                damage *= (1 - damageDealtReduction);
            }

            if (target.GetCachedComponent<IModifiable>().GetModifiers().Any(x => x is DamageDealtReductionModifierDefinition.Modifier))
            {
                damage += GetCachedComponent<IModifiable>().GetModifiers().Sum(x => x.DamageDealtAgainstWeak ?? 0);
            }

            return new Attack(new AttackSource(this).Add(source), damage, armorPenetration, leach, ranged, empowered);
        }

        public ITargeteable GetTarget(TargetCriteria criteria, object caller)
        {
            return GetTargets(criteria, caller).FirstOrDefault();
        }

        public List<ITargeteable> GetTargets(TargetCriteria criteria, object caller)
        {
            List<ITargeteable> potentialTargets = new List<ITargeteable>();
            foreach (ITargeteable attackable in AgentObject.All.Select(x => x.GetCachedComponent<ITargeteable>()).Where(x => x != null))
            {
                if (!attackable.IsActive)
                    continue;

                if (!criteria.Execute(this.GetCachedComponent<ITargeteable>(), attackable, caller))
                    continue;

                potentialTargets.Add(attackable);
            }

            return potentialTargets
                .OrderBy(x => x.Priority)
                .ToList();
        }

        public void Death()
        {
            EventChannelDeath.Instance.Publish(new EventChannelDeath.Event() { AgentObject = this });
            stateMachine.SetState(new DeathState(this));
        }

        public void Displace(Vector2 displacement)
        {
            rigidbody.MovePosition(this.rigidbody.position + displacement);
        }

        public void Stagger(float duration)
        {
            foreach (Ability ability in abilities)
            {
                if (ability.IsActive)
                    ability.Interrupt();
            }

            stateMachine.SetState(new StaggerState(this, duration));
        }

        public void Heal(float amount)
        {
            this.Health += amount;
            this.Health = Mathf.Clamp(Health, 0, MaxHealth);
        }

        public void AttackLanded(AttackResult attackResult)
        {
            Heal(attackResult.DamageTaken * attackResult.Attack.Leach);
            OnAttackLanded?.Invoke(attackResult);
        }

        #region Ability
        [Header("Abilities")]
        [SerializeField] private List<AbilityDefinition> abilitiesDefinition = new List<AbilityDefinition>();

        private List<Ability> abilities = new List<Ability>();

        public List<Ability> Abilities { get => abilities; set => abilities = value; }
        public float LastAbilityUsed { get; set; }

        private void InitializeAbilities()
        {
            GameObject abilitiesParent = new GameObject("Abilities");
            abilitiesParent.transform.parent = transform;

            foreach (AbilityDefinition definition in abilitiesDefinition)
            {
                Ability ability = definition.GetAbility();
                ability.transform.parent = abilitiesParent.transform;
                ability.Initialize(this);

                abilities.Add(ability);
            }
        }

        private void UpdateAbilities()
        {
            foreach (Ability ability in abilities)
            {
                if (ability.IsActive)
                    ability.Tick();
            }
        }

        public Ability GetCurrentAbility()
        {
            return abilities.FirstOrDefault(a => a.IsActive);
        }

        public void Cast()
        {
            stateMachine.SetState(new CastingState(this));
        }

        public void EndCast()
        {
            stateMachine.SetState(new MoveState(this));
        }

        public bool CanUseAbility()
        {
            if (Health <= 0 || IsDead)
                return false;

            return true;
        }

        private void DisposeAbilities()
        {
            foreach (Ability ability in abilities)
                ability.Dispose();
        }

        #endregion
    }
}