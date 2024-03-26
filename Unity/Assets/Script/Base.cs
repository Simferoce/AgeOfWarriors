using UnityEngine;

namespace Game
{
    public class Base : AgentObject, ITargeteable
    {
        [SerializeField] private float health;
        [SerializeField] private float maxHealth;
        [SerializeField] private SpawnPoint spawnPoint;

        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }

        public Faction Faction => Agent.Faction;

        public int Priority => int.MaxValue;

        private void Awake()
        {
            health = maxHealth;
        }

        public void TakeAttack(float damage)
        {
            health -= damage;
        }

        public bool Attackable()
        {
            return this.health > 0;
        }

        public bool CanBlocks(Faction faction)
        {
            return faction != this.Faction;
        }
    }
}

