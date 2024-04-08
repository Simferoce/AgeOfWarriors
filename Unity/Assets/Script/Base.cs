using UnityEngine;

namespace Game
{
    public class Base : AgentObject
    {
        [SerializeReference, SubclassSelector] private IStatisticFloat maxHealth;
        [SerializeReference, SubclassSelector] private IStatisticFloat defense;
        [SerializeField] private SpawnPoint spawnPoint;

        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
        public override IStatisticFloat MaxHealth => maxHealth;
        public override IStatisticFloat Defense => defense;
    }
}

