using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public partial class Character : AgentObject<CharacterDefinition>, IDisplaceable
    {
        [Header("Collision")]
        [SerializeField] private new Rigidbody2D rigidbody;

        [Header("Abilities")]
        [SerializeField] private List<AbilityDefinition> abilitiesDefinition = new List<AbilityDefinition>();

        public override float MaxHealth { get => Definition.MaxHealth + modifierHandler.MaxHealth ?? 0; }
        public override float Defense { get => Definition.Defense + modifierHandler.Defense ?? 0f; }
        public override float AttackSpeed { get => Definition.AttackPerSeconds; }
        public override float AttackPower { get => Definition.AttackPower + modifierHandler.AttackPower ?? 0f; }
        public override float Speed { get => Definition.Speed * (1 + modifierHandler.SpeedPercentage ?? 0f); }
        public override float Reach { get => Definition.Reach; }

        public override bool IsDead { get => stateMachine.Current is DeathState; }
        public CharacterAnimator CharacterAnimator { get; set; }
        public float LastAbilityUsed { get; set; }
        public override bool IsActive { get => !IsDead; }
        public override bool IsEngaged() => GetTarget(engagedCriteria, this) != null;
        public override bool IsInvulnerable => modifierHandler.Invulnerable ?? false;
        public List<Ability> Abilities { get => abilities; set => abilities = value; }

        private StateMachine stateMachine = new StateMachine();
        private TargetCriteria engagedCriteria = new IsEnemyTargetCriteria();
        private List<Ability> abilities = new List<Ability>();

        public override bool TryGetStatisticValue<T>(StatisticDefinition statisticDefinition, out T value)
        {
            if (statisticDefinition == StatisticDefinition.MaxHealth)
            {
                value = (T)(object)MaxHealth;
                return true;
            }
            else if (statisticDefinition == StatisticDefinition.Defense)
            {
                value = (T)(object)Defense;
                return true;
            }
            else if (statisticDefinition == StatisticDefinition.AttackPower)
            {
                value = (T)(object)AttackPower;
                return true;
            }
            else if (statisticDefinition == StatisticDefinition.AttackSpeed)
            {
                value = (T)(object)AttackSpeed;
                return true;
            }
            else if (statisticDefinition == StatisticDefinition.Speed)
            {
                value = (T)(object)Speed;
                return true;
            }
            else if (statisticDefinition == StatisticDefinition.Reach)
            {
                value = (T)(object)Reach;
                return true;
            }

            return base.TryGetStatisticValue<T>(statisticDefinition, out value);
        }

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            CharacterAnimator = GetComponentInChildren<CharacterAnimator>();

            stateMachine.Initialize(new MoveState(this));
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

        public void FixedUpdate()
        {
            if (IsDead)
                return;

            foreach (Ability ability in abilities)
            {
                if (ability.IsActive)
                    ability.Update();
            }

            stateMachine.Update();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (Ability ability in abilities)
                ability.Dispose();
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

        public IAttackable GetTarget(TargetCriteria criteria, object caller)
        {
            return GetTargets(criteria, caller).FirstOrDefault();
        }

        public List<IAttackable> GetTargets(TargetCriteria criteria, object caller)
        {
            List<IAttackable> potentialTargets = new List<IAttackable>();
            foreach (IAttackable attackable in AgentObject.All.OfType<IAttackable>())
            {
                if (!attackable.IsActive)
                    continue;

                if (!criteria.Execute(this, attackable, caller))
                    continue;

                potentialTargets.Add(attackable);
            }

            return potentialTargets
                .OrderBy(x => x.Priority)
                .ToList();
        }

        protected override void InternalDeath()
        {
            stateMachine.SetState(new DeathState(this));
        }

        public void Displace(Vector2 displacement)
        {
            rigidbody.MovePosition(this.rigidbody.position + displacement);
        }

        public override void Stagger(float duration)
        {
            foreach (Ability ability in abilities)
            {
                if (ability.IsActive)
                    ability.Interrupt();
            }

            stateMachine.SetState(new StaggerState(this, duration));
        }
    }
}