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

        //[SerializeField] private StatisticDynamicFloatCharacter maxHealth = new StatisticDynamicFloatCharacter((o, c) => o.Definition.MaxHealth.GetValue(c) + o.modifierHandler.MaxHealth ?? 0);
        //[SerializeField] private StatisticDynamicFloatCharacter defense = new StatisticDynamicFloatCharacter((o, c) => o.Definition.Defense.GetValue(c) + o.modifierHandler.Defense ?? 0f);
        //[SerializeField] private StatisticDynamicFloatCharacter attackSpeed = new StatisticDynamicFloatCharacter((o, c) => o.Definition.AttackPerSeconds.GetValue(c));
        //[SerializeField] private StatisticDynamicFloatCharacter attackPower = new StatisticDynamicFloatCharacter((o, c) => o.Definition.AttackPower.GetValue(c) + o.modifierHandler.AttackPower ?? 0f);
        //[SerializeField] private StatisticDynamicFloatCharacter speed = new StatisticDynamicFloatCharacter((o, c) => o.Definition.Speed.GetValue(c) * (1 + o.modifierHandler.SpeedPercentage ?? 0f));
        //[SerializeField] private StatisticDynamicFloatCharacter reach = new StatisticDynamicFloatCharacter((o, c) => o.Definition.Reach.GetValue(c));

        [SerializeField] private StatisticHolder statistics;
        public StatisticHolder Statistics => statistics;

        [SerializeField] private StatisticSerializeFloat maxHealth = new StatisticSerializeFloat(10);
        [SerializeField] private StatisticSerializeFloat defense = new StatisticSerializeFloat(5);
        [SerializeField] private StatisticSerializeFloat attackSpeed = new StatisticSerializeFloat(1);
        [SerializeField] private StatisticSerializeFloat attackPower = new StatisticSerializeFloat(1);
        [SerializeField] private StatisticSerializeFloat speed = new StatisticSerializeFloat(1);
        [SerializeField] private StatisticSerializeFloat reach = new StatisticSerializeFloat(1);

        public override IStatisticFloat MaxHealth => maxHealth;
        public override IStatisticFloat Defense => defense;
        public override IStatisticFloat AttackSpeed => attackSpeed;
        public override IStatisticFloat AttackPower => attackPower;
        public override IStatisticFloat Speed => speed;
        public override IStatisticFloat Reach => reach;

        public override bool IsDead { get => stateMachine.Current is DeathState; }
        public CharacterAnimator CharacterAnimator { get; set; }
        public float LastAbilityUsed { get; set; }
        public override bool IsActive { get => !IsDead; }
        public override bool IsEngaged() => GetTarget(engagedCriteria, new StatisticContext()) != null;
        public override bool IsInvulnerable => modifierHandler.Invulnerable ?? false;
        public List<Ability> Abilities { get => abilities; set => abilities = value; }

        private StateMachine stateMachine = new StateMachine();
        private TargetCriteria engagedCriteria = new IsEnemyTargetCriteria();
        private List<Ability> abilities = new List<Ability>();

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            maxHealth.Initialize(this);
            defense.Initialize(this);
            attackSpeed.Initialize(this);
            attackPower.Initialize(this);
            speed.Initialize(this);
            reach.Initialize(this);

            base.Spawn(agent, spawnNumber, direction);

            CharacterAnimator = GetComponentInChildren<CharacterAnimator>();
            stateMachine.Initialize(new MoveState(this));

            GameObject abilitiesHolder = new GameObject("Abilities");
            abilitiesHolder.transform.parent = transform;
            foreach (AbilityDefinition definition in abilitiesDefinition)
            {
                Ability characterAbility = definition.GetAbility();
                characterAbility.transform.parent = abilitiesHolder.transform;
                characterAbility.Initialize(this);

                abilities.Add(characterAbility);
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

        public IAttackable GetTarget(TargetCriteria criteria, StatisticContext context)
        {
            return GetTargets(criteria, context).FirstOrDefault();
        }

        public List<IAttackable> GetTargets(TargetCriteria criteria, StatisticContext context)
        {
            List<IAttackable> potentialTargets = new List<IAttackable>();
            foreach (IAttackable attackable in AgentObject.All.OfType<IAttackable>())
            {
                if (!attackable.IsActive)
                    continue;

                if (!criteria.Execute(this, attackable, context))
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