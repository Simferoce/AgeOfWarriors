using UnityEngine;

namespace Game
{
    public class Base : LaneObject, ITargeteable
    {
        [SerializeField] private float health;
        [SerializeField] private float maxHealth;
        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private Lane lane;

        private void Awake()
        {
            health = maxHealth;
        }

        private void Start()
        {
            spawnPoint.Initialize(lane);
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

