using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public partial class Character : AgentObject<CharacterDefinition>, IBlocker, IDisplaceable, IAttackSource, IModifiable, IAttackable
    {
        [Header("Collision")]
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private Collider2D hitbox;
        [SerializeField] private Transform targetPosition;

        [Header("Abilities")]
        [SerializeReference, SubclassSelector]
        private List<CharacterAbility> abilities = new List<CharacterAbility>();

        public float MaxHealth { get => Definition.MaxHealth; }
        public float Health { get => health; set => health = value; }
        public float AttackPerSeconds { get => Definition.AttackPerSeconds; }
        public float AttackPower { get => Definition.AttackPower; }
        public float Speed { get => Definition.Speed * (1 + ModifierHandler.SpeedPercentage ?? 0f); }
        public float Defense { get => Definition.Defense + ModifierHandler.Defense ?? 0f; }
        public float Reach { get => Definition.Reach; }
        public ModifierHandler ModifierHandler { get; } = new ModifierHandler();
        public int Priority { get => SpawnNumber; }
        public Faction Faction { get => Agent.Faction; }
        public bool IsDead { get => stateMachine.Current is DeathState; }
        public CharacterAnimator CharacterAnimator { get; set; }
        public Vector3 TargetPosition => targetPosition.position;
        public Vector3 Position => transform.position;
        public CharacterDefinition CharacterDefinition { get; set; }
        public float LastAbilityUsed { get; set; }
        public Collider2D Collider { get => hitbox; }
        public bool IsActive { get => !IsDead; }
        public bool IsDisplaceable => true;
        public event Action<Attack, IAttackable> OnDamageTaken;

        private float health;
        private StateMachine stateMachine = new StateMachine();

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            stateMachine.Initialize(new MoveState(this));
            CharacterAnimator = GetComponentInChildren<CharacterAnimator>();
            health = MaxHealth;

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

            ModifierHandler.Dispose();
        }

        public bool IsBlocking(Faction faction)
        {
            return true;
        }

        public void Heal(float amount)
        {
            this.health += amount;
            this.health = Mathf.Clamp(health, 0, MaxHealth);
        }

        public void Cast()
        {
            stateMachine.SetState(new CastingState(this));
        }

        public void EndCast()
        {
            stateMachine.SetState(new MoveState(this));
        }

        #region Attack

        public bool CanUseAbility()
        {
            if (health <= 0 || IsDead)
                return false;

            return true;
        }

        public IAttackable GetTarget()
        {
            return GetTargets().FirstOrDefault();
        }

        public Vector3 ClosestPoint(Vector3 point)
        {
            return Collider.ClosestPoint(this.TargetPosition);
        }

        public List<IAttackable> GetTargets()
        {
            List<IAttackable> potentialTargets = new List<IAttackable>();
            foreach (IAttackable attackable in AgentObject.All.OfType<IAttackable>())
            {
                if (!attackable.IsActive)
                    continue;

                if (attackable.Faction == this.Agent.Faction)
                    continue;

                if (Mathf.Abs((attackable.ClosestPoint(this.TargetPosition) - this.TargetPosition).x) > Reach)
                    continue;

                if (attackable.Equals(this))
                    continue;

                if (!attackable.Attackable())
                    continue;

                potentialTargets.Add(attackable);
            }

            return potentialTargets
                .OrderBy(x => x.Priority)
                .ToList();
        }

        public bool Attackable()
        {
            return this.health > 0;
        }

        public void AttackLanded(Attack attack, float damageDealt)
        {
            Heal(damageDealt * attack.Leach);
        }

        public void TakeAttack(Attack attack)
        {
            float damageReduced = DefenseFormulaDefinition.Instance.ParseDamage(attack.Damage, Mathf.Max(0, Defense - attack.ArmorPenetration));
            this.health -= damageReduced;

            foreach (IAttackSource source in attack.AttackSource.Sources)
                source.AttackLanded(attack, damageReduced);

            Debug.Log($"{this.name} took {damageReduced} from {attack.AttackSource.Sources[^1]}");
            OnDamageTaken?.Invoke(attack, this);

            if (health <= 0 && !IsDead)
                Death();
        }

        public void Death()
        {
            stateMachine.SetState(new DeathState(this));
        }

        public void AddModifier(Modifier modifier)
        {
            ModifierHandler.Add(modifier);
        }

        public void RemoveModifier(Modifier modifier)
        {
            ModifierHandler.Remove(modifier);
        }

        public List<Modifier> GetModifiers()
        {
            return ModifierHandler.Modifiers;
        }

        public void Displace(Vector2 displacement)
        {
            rigidbody.MovePosition(this.rigidbody.position + displacement);
        }

        public void Stagger(float duration)
        {
            foreach (CharacterAbility ability in abilities)
            {
                if (ability.IsActive)
                    ability.Interrupt();
            }

            stateMachine.SetState(new StaggerState(this, duration));
        }

        #endregion
    }
}