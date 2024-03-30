using Character;
using Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Character : AgentObject<CharacterDefinition>, ITargeteable, IDamageSource, IModifiable, IAttackable
    {
        [Header("Collision")]
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private DetectionCollision hitBox;
        [SerializeField] private DetectionCollision hitZone;
        [SerializeField] private Transform targetPosition;
        [SerializeField] private ReachHandler reachHandler;

        [Header("Abilities")]
        [SerializeReference, SubclassSelector]
        private List<CharacterAbility> abilities = new List<CharacterAbility>();

        public float MaxHealth { get => Definition.MaxHealth; }
        public float Health { get => health; set => health = value; }
        public float AttackPerSeconds { get => Definition.AttackPerSeconds; }
        public float AttackPower { get => Definition.AttackPower; }
        public float Speed { get => Definition.Speed * (1 + ModifierHandler.SpeedPercentage ?? 0f); }
        public float Defense { get => Definition.Defense + ModifierHandler.Defense ?? 0f; }
        public ModifierHandler ModifierHandler { get; } = new ModifierHandler();
        public int Priority { get => SpawnNumber; }
        public Faction Faction { get => Agent.Faction; }
        public bool IsDead { get; set; } = false;
        public CharacterAnimator CharacterAnimator { get; set; }
        public Vector3 Position => targetPosition.position;
        public CharacterDefinition CharacterDefinition { get; set; }
        public float LastAbilityUsed { get; set; }

        public event Action<DamageSource, IAttackable> OnDamageTaken;

        private float health;

        public void Update()
        {
            foreach (CharacterAbility ability in abilities)
            {
                if (ability.IsActive)
                    ability.Update();
            }
        }

        public void FixedUpdate()
        {
            if (IsDead)
                return;

            if (CanUseAbility())
            {
                foreach (CharacterAbility ability in abilities)
                {
                    if (ability.CanUse())
                    {
                        ability.Use();
                        break;
                    }
                }
            }

            if (CanMove())
            {
                this.CharacterAnimator.SetFloat(CharacterAnimatorParameter.Parameter.SpeedRatio, 1, 0.25f);
                rigidbody.MovePosition(this.rigidbody.position + Vector2.right * Direction * Speed * Time.deltaTime);
            }
            else
            {
                this.CharacterAnimator.SetFloat(CharacterAnimatorParameter.Parameter.SpeedRatio, 0f, 0.25f);
            }
        }

        protected override void OnDestroy()
        {
            foreach (CharacterAbility ability in abilities)
                ability.Dispose();

            ModifierHandler.Dispose();
        }

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            CharacterAnimator = GetComponentInChildren<CharacterAnimator>();
            health = MaxHealth;

            foreach (CharacterAbility ability in abilities)
            {
                ability.Initialize(this);
            }

            reachHandler.SetReach(Definition.Reach);
        }

        private bool CanMove()
        {
            if (abilities.Any(x => x.IsCasting))
                return false;

            foreach (GameObject collision in hitBox.InCollisions)
            {
                if (!collision.CompareTag(GameTag.HIT_BOX))
                    continue;

                if (!collision.TryGetComponentInParent<ITargeteable>(out ITargeteable targeteable))
                    continue;

                if (targeteable == (this as ITargeteable))
                    continue;

                if (!targeteable.CanBlocks(this.Faction))
                    continue;

                if (targeteable.Faction != this.Faction)
                    return false;

                if (targeteable.Priority < this.Priority)
                    return false;
            }

            return true;
        }

        public bool CanBlocks(Faction faction)
        {
            return true;
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

        public List<IAttackable> GetTargets()
        {
            List<IAttackable> potentialTargets = new List<IAttackable>();
            foreach (GameObject inRange in hitZone.InCollisions)
            {
                if (!inRange.CompareTag(GameTag.HIT_BOX))
                    continue;

                if (!inRange.TryGetComponentInParent<IAttackable>(out IAttackable attackable))
                    continue;

                if (attackable.Equals(this))
                    continue;

                if (!attackable.Attackable())
                    continue;

                if (attackable.Faction != this.Agent.Faction)
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

        public void TakeAttack(DamageSource source, float damage)
        {
            float damageReduced = DefenseFormulaDefinition.Instance.ParseDamage(damage, Defense);
            this.health -= damageReduced;
            Debug.Log($"{this.name} took {damageReduced} from {source.Sources[^1]}");

            OnDamageTaken?.Invoke(source, this);

            if (health <= 0 && !IsDead)
                Death();
        }

        public void Death()
        {
            IsDead = true;
            this.CharacterAnimator.SetTrigger(CharacterAnimatorParameter.Parameter.Dead);

            hitBox.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject, 2);
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

        #endregion
    }
}