using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Character : LaneObject, ITargeteable
    {
        [SerializeField] private float attackRange;
        [SerializeField] private float health;
        [SerializeField] private float maxHealth;
        [SerializeField] private float speed;

        private CharacterAnimator characterAnimator = null;
        private bool isAttacking = false;

        public CharacterAnimator CharacterAnimator { get => characterAnimator; }
        public float Speed { get => speed; set => speed = value; }
        public float MaxHealth { get => maxHealth; set => maxHealth = value; }
        public float Health { get => health; set => health = value; }
        public float AttackRange { get => attackRange; set => attackRange = value; }

        private void Start()
        {
            health = maxHealth;
            characterAnimator = GetComponentInChildren<CharacterAnimator>();
            characterAnimator.Attacked += OnAttacked;
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(this.transform.position + Vector3.right * attackRange, 0.02f);
            Gizmos.DrawSphere(this.transform.position - Vector3.right * attackRange, 0.02f);
            Gizmos.DrawLine(this.transform.position + Vector3.right * attackRange, this.transform.position - Vector3.right * attackRange);
        }

        public override void Tick()
        {
            UpdateMovement();

            if (CanAttack() && !InAttackRange())
            {
                isAttacking = false;
                this.CharacterAnimator.SetLayerWeight(CharacterAnimator.LAYER_UPPER_BODY, 0f);
            }
            else
            {
                if (isAttacking == false)
                {
                    this.CharacterAnimator.SetTrigger(CharacterAnimator.ATTACK);
                    this.CharacterAnimator.SetLayerWeight(CharacterAnimator.LAYER_UPPER_BODY, 1f);
                }

                isAttacking = true;
            }
        }

        #region Attack
        public bool InAttackRange()
        {
            float attackRangeMin = this.Direction > 0 ? this.Position : this.Position - this.attackRange;
            float attackRangeMax = this.Direction > 0 ? this.Position + this.attackRange : this.Position;
            List<ITargeteable> intersectedObjects = this.Lane.Intersecting(attackRangeMin, attackRangeMax)
                .Where(x => x != this && (x.Agent == null || this.Agent == null || x.Agent.Faction != this.Agent.Faction))
                .Select(x => x.GetComponent<ITargeteable>())
                .Where(x => x != null && x.Attackable(this.gameObject)).ToList();
            ITargeteable intersecting = intersectedObjects.FirstOrDefault();

            return intersecting != null;
        }

        public bool CanAttack()
        {
            return health > 0;
        }

        public void OnAttacked()
        {
            if (!CanAttack())
                return;

            List<(float, ITargeteable)> inRange = Lane.CastAll<LaneObject>(this.Position, this.Position + this.Direction * this.attackRange)
                .Where(x => x.Item2.Agent == null || this.Agent == null || this.Agent.Faction != x.Item2.Agent.Faction)
                .Select(x => (x.Item1, x.Item2.GetComponent<ITargeteable>()))
                .Where(x => x.Item2 != null && x.Item2.Attackable(this.gameObject))
                .ToList();

            if (inRange.Count > 0)
            {
                (_, ITargeteable target) = inRange.OrderBy(x => x.Item1).First();
                target.Attack(1);
            }
        }

        public bool Attackable(GameObject from)
        {
            return this.health > 0;
        }

        public void Attack(float damage)
        {
            this.health -= damage;

            if (health <= 0)
                GameObject.Destroy(this.gameObject);
        }
        #endregion

        #region Movement

        public void UpdateMovement()
        {
            IEnumerable<LaneObject> intersectingObjects = this.Lane.Intersecting(this.Position - this.CollisionRange, this.Position + this.CollisionRange);
            List<LaneObject> filteredIntersectingObjects = FilterCollision(intersectingObjects).ToList();

            if (filteredIntersectingObjects.Count() == 0)
            {
                this.CharacterAnimator.SetFloat(CharacterAnimator.SPEED_RATIO, 1);
                Move();
            }
            else
            {
                this.CharacterAnimator.SetFloat(CharacterAnimator.SPEED_RATIO, 0.0f);
            }
        }

        public void Move()
        {
            float translation = this.Speed * this.Direction * Time.deltaTime;

            float border = this.Direction > 0 ? this.Max : this.Min;
            if (this.Lane.Cast<LaneObject>(border, border + translation, out float hit))
            {
                this.Position = hit - (this.Direction > 0 ? this.Max - this.Position : this.Min - this.Position);
            }
            else
            {
                this.Position += translation;
            }
        }

        public List<LaneObject> FilterCollision(IEnumerable<LaneObject> collisions)
        {
            List<LaneObject> results = new List<LaneObject>();
            foreach (LaneObject collision in collisions)
            {
                if (this.Agent == null || collision.Agent == null)
                {
                    results.Add(collision);
                }
                else if (collision.Agent.Faction != this.Agent.Faction)
                {
                    results.Add(collision);
                }
                else if (collision.Agent == this.Agent && collision is Character intersectingCharacter && intersectingCharacter.SpawnNumber < this.SpawnNumber)
                {
                    results.Add(collision);
                }
            }

            return results;
        }
        #endregion
    }
}