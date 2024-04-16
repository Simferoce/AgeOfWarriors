using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Target))]
    [RequireComponent(typeof(Attackable))]
    public class Base : AgentObject, IAttackableOwner, ITargetOwner
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float defense;
        [SerializeField] private SpawnPoint spawnPoint;

        public float Health { get; set; }
        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
        public float MaxHealth => maxHealth;
        public float Defense => defense;
        public bool IsDead => Health <= 0;
        public bool IsInvulnerable => false;

        public void Death()
        {
            EventChannelDeath.Instance.Publish(new EventChannelDeath.Event() { AgentObject = this });
            Destroy(this.gameObject);
        }

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            Health = maxHealth;
        }
    }
}

