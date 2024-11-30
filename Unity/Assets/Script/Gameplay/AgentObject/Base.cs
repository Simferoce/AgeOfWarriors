using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Attackable))]
    [RequireComponent(typeof(Target))]
    public class Base : AgentObject, IBlock
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float defense;
        [SerializeField] private SpawnPoint spawnPoint;

        [Header("Target")]
        [SerializeField] private Collider2D hitbox;
        [SerializeField] private Transform targetPosition;

        public float Health { get; set; }
        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
        public Vector3 CenterPosition => transform.position;
        public Vector3 TargetPosition => targetPosition.position;
        public float MaxHealth => maxHealth;
        public float Defense => defense;
        public bool IsDead => Health <= 0;

        protected override void Awake()
        {
            base.Awake();
            GetCachedComponent<Attackable>().OnAttackTaken += Base_OnAttackTaken;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetCachedComponent<Attackable>().OnAttackTaken -= Base_OnAttackTaken;
        }

        private void Base_OnAttackTaken(AttackResult attackResult)
        {
            Health -= attackResult.DamageTaken;

            if (Health <= 0 && !IsDead)
                Death();
        }

        public Vector3 ClosestPoint(Vector3 point)
        {
            return hitbox.ClosestPoint(point);
        }

        public void Death()
        {
            DeathEventChannel.Instance.Publish(new DeathEventChannel.Event() { AgentObject = this });
            Destroy(this.gameObject);
        }

        public bool IsBlocking(Character character)
        {
            return hitbox.IsTouching(character.Hitbox) &&
                character.OriginalFaction != this.OriginalFaction;
        }

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            Health = maxHealth;
        }
    }
}

