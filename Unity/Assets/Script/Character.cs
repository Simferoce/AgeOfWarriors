using Character;
using Extension;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Character : AgentObject, ITargeteable
    {
        [Header("Stats")]
        [SerializeField] private float speed;
        [SerializeField] private float maxHealth;

        [Header("Collision")]
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private DetectionCollision hitBox;
        [SerializeField] private DetectionCollision hitZone;

        public float Speed { get => speed; set => speed = value; }
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float Health { get => health; set => health = value; }
        public int Priority { get => SpawnNumber; }
        public Faction Faction { get => Agent.Faction; }

        private CharacterAnimator characterAnimator = null;
        private bool isAttacking = false;
        private bool isDead = false;
        private float health;

        private void Start()
        {
            health = MaxHealth;
            characterAnimator = GetComponentInChildren<CharacterAnimator>();
            characterAnimator.Attacked += OnAttacked;
        }

        public void FixedUpdate()
        {
            if (isDead)
                return;

            if (CanMove())
            {
                this.characterAnimator.SetFloat(CharacterAnimator.SPEED_RATIO, 1);
                rigidbody.MovePosition(this.rigidbody.position + Vector2.right * Direction * Speed * Time.deltaTime);
            }
            else
            {
                this.characterAnimator.SetFloat(CharacterAnimator.SPEED_RATIO, 0f);
            }

            if (CanAttack())
            {
                if (isAttacking == false)
                {
                    this.characterAnimator.SetTrigger(CharacterAnimator.ATTACK);
                    this.characterAnimator.SetLayerWeight(CharacterAnimator.LAYER_UPPER_BODY, 1f);
                }

                isAttacking = true;
            }
            else
            {
                isAttacking = false;
                this.characterAnimator.SetLayerWeight(CharacterAnimator.LAYER_UPPER_BODY, 0f);
            }
        }

        private bool CanMove()
        {
            foreach (GameObject collision in hitBox.InCollisions)
            {
                if (!collision.CompareTag(GameTag.HIT_BOX))
                    continue;

                if (!collision.TryGetComponentInParent<AgentObject>(out AgentObject laneObject))
                    continue;

                if (laneObject == this)
                    continue;

                if (laneObject.Agent.Faction != this.Agent.Faction)
                    return false;

                if (laneObject.SpawnNumber > this.SpawnNumber)
                    return false;
            }

            return true;
        }

        #region Attack
        public bool CanAttack()
        {
            if (health <= 0 || isDead)
                return false;

            return GetTarget() != null;
        }

        public void OnAttacked()
        {
            if (!CanAttack())
                return;

            ITargeteable target = GetTarget();

            if (target != null)
                target.Attack(1);
        }

        public ITargeteable GetTarget()
        {
            List<ITargeteable> potentialTargets = new List<ITargeteable>();
            foreach (GameObject inRange in hitZone.InCollisions)
            {
                if (!inRange.CompareTag(GameTag.HIT_BOX))
                    continue;

                if (!inRange.TryGetComponentInParent<ITargeteable>(out ITargeteable targeteable))
                    continue;

                if (targeteable.Equals(this))
                    continue;

                if (!targeteable.Attackable())
                    continue;

                if (targeteable.Faction != this.Agent.Faction)
                    potentialTargets.Add(targeteable);
            }

            return potentialTargets
                .OrderBy(x => x.Priority)
                .FirstOrDefault();
        }

        public bool Attackable()
        {
            return this.health > 0;
        }

        public void Attack(float damage)
        {
            this.health -= damage;

            if (health <= 0)
                Death();
        }

        public void Death()
        {
            isDead = true;
            this.characterAnimator.SetLayerWeight(CharacterAnimator.LAYER_UPPER_BODY, 0f);
            this.characterAnimator.SetTrigger(CharacterAnimator.DEAD);

            hitBox.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject, 2);
        }

        #endregion
    }
}