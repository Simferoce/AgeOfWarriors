using UnityEngine;

namespace Game
{
    public class Wall : AgentObject
    {
        [SerializeReference, SerializeReferenceDropdown] private IStatisticFloat maxHealth;
        [SerializeReference, SerializeReferenceDropdown] private IStatisticFloat defense;

        public override IStatisticFloat MaxHealth => maxHealth;
        public override IStatisticFloat Defense => defense;
    }
}