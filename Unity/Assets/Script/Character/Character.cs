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
        [SerializeReference, SubclassSelector]
        private List<CharacterAbility> abilities = new List<CharacterAbility>();

        public override float MaxHealth { get => Definition.MaxHealth; }
        public override float Defense { get => Definition.Defense + modifierHandler.Defense ?? 0f; }
        public float AttackPerSeconds { get => Definition.AttackPerSeconds; }
        public float AttackPower { get => Definition.AttackPower; }
        public float Speed { get => Definition.Speed * (1 + modifierHandler.SpeedPercentage ?? 0f); }
        public float Reach { get => Definition.Reach; }
        public override bool IsDead { get => stateMachine.Current is DeathState; }
        public CharacterAnimator CharacterAnimator { get; set; }
        public CharacterDefinition CharacterDefinition { get; set; }
        public float LastAbilityUsed { get; set; }
        public override bool IsActive { get => !IsDead; }
        public bool IsDisplaceable => true;

        private StateMachine stateMachine = new StateMachine();

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            stateMachine.Initialize(new MoveState(this));
            CharacterAnimator = GetComponentInChildren<CharacterAnimator>();

            foreach (CharacterAbility ability in abilities)
            {
                ability.Initialize(this);
            }
        }

        public void FixedUpdate()
        {
            if (IsDead)
                return;

            foreach (CharacterAbility ability in abilities)
            {
                if (ability.IsActive)
                    ability.Update();
            }

            stateMachine.Update();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (CharacterAbility ability in abilities)
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

        public IAttackable GetTarget(List<Tag> target, float distance)
        {
            return GetTargets(target, distance).FirstOrDefault();
        }

        public List<IAttackable> GetTargets(List<Tag> tags, float distance)
        {
            List<IAttackable> potentialTargets = new List<IAttackable>();
            foreach (IAttackable attackable in AgentObject.All.OfType<IAttackable>())
            {
                if (!attackable.IsActive)
                    continue;

                if (!MatchAll(tags, attackable))
                    continue;

                if (Mathf.Abs((attackable.ClosestPoint(this.Position) - this.Position).x) > distance)
                    continue;

                if (attackable.Equals(this))
                    continue;

                if (!attackable.IsAttackable())
                    continue;

                potentialTargets.Add(attackable);
            }

            return potentialTargets
                .OrderBy(x => x.Priority)
                .ToList();
        }

        public override void Death()
        {
            stateMachine.SetState(new DeathState(this));
        }

        public void Displace(Vector2 displacement)
        {
            rigidbody.MovePosition(this.rigidbody.position + displacement);
        }

        public override void Stagger(float duration)
        {
            foreach (CharacterAbility ability in abilities)
            {
                if (ability.IsActive)
                    ability.Interrupt();
            }

            stateMachine.SetState(new StaggerState(this, duration));
        }
    }
}