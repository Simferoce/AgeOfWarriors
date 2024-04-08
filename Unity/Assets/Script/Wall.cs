using UnityEngine;

namespace Game
{
    public class Wall : AgentObject
    {
        [SerializeReference, SubclassSelector] private IStatisticFloat maxHealth;
        [SerializeReference, SubclassSelector] private IStatisticFloat defense;

        public override IStatisticFloat MaxHealth => maxHealth;
        public override IStatisticFloat Defense => defense;
    }
}