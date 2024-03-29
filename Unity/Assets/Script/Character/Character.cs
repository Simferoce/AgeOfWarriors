﻿using Character;
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
        [SerializeField] private Transform targetPosition;

        public float Speed { get => speed; set => speed = value; }
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float Health { get => health; set => health = value; }
        public int Priority { get => SpawnNumber; }
        public Faction Faction { get => Agent.Faction; }
        public bool IsDead { get; set; } = false;
        public CharacterAnimator CharacterAnimator { get; set; }
        public Vector3 Position => targetPosition.position;

        [SerializeReference, SubclassSelector]
        private CharacterAbility ability = null;
        private float health;

        private void Start()
        {
            CharacterAnimator = GetComponentInChildren<CharacterAnimator>();

            health = MaxHealth;
            ability.Initialize(this);
        }

        private void OnDestroy()
        {
            ability.Dispose();
        }

        public void FixedUpdate()
        {
            if (IsDead)
                return;

            if (CanMove())
            {
                this.CharacterAnimator.SetFloat(CharacterAnimator.SPEED_RATIO, 1);
                rigidbody.MovePosition(this.rigidbody.position + Vector2.right * Direction * Speed * Time.deltaTime);
            }
            else
            {
                this.CharacterAnimator.SetFloat(CharacterAnimator.SPEED_RATIO, 0f);
            }

            if (CanUseAbility())
            {
                ability.Use();
            }
        }

        private bool CanMove()
        {
            if (ability.IsCasting)
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
            if (!ability.CanUse())
                return false;

            if (health <= 0 || IsDead)
                return false;

            return true;
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

        public void TakeAttack(float damage)
        {
            this.health -= damage;

            if (health <= 0)
                Death();
        }

        public void Death()
        {
            IsDead = true;
            this.CharacterAnimator.SetTrigger(CharacterAnimator.DEAD);

            hitBox.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject, 2);
        }



        #endregion
    }
}