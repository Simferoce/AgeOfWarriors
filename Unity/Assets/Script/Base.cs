using UnityEngine;

namespace Game
{
    public class Base : LaneObject, ITargeteable
    {
        [SerializeField] private float health;
        [SerializeField] private float maxHealth;
        [SerializeField] private SpawnPoint spawnPoint;

        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

        private void Awake()
        {
            health = maxHealth;
        }

        public void Attack(float damage)
        {
            health -= damage;
        }

        public bool Attackable(GameObject from)
        {
            return this.health > 0;
        }
    }
}

