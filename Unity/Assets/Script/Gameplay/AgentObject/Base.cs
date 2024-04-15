using UnityEngine;

namespace Game
{
    public class Base : AgentObject
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float defense;
        [SerializeField] private SpawnPoint spawnPoint;

        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
        public override float MaxHealth => maxHealth;
        public override float Defense => defense;
    }
}

