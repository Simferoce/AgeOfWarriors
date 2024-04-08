using UnityEngine;

namespace Game
{
    public class Base : AgentObject
    {
        [SerializeReference, SerializeReferenceDropdown] private IStatisticFloat maxHealth;
        [SerializeReference, SerializeReferenceDropdown] private IStatisticFloat defense;
        [SerializeField] private SpawnPoint spawnPoint;

        public SpawnPoint SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
        public override IStatisticFloat MaxHealth => maxHealth;
        public override IStatisticFloat Defense => defense;
    }
}

