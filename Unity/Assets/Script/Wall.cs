using UnityEngine;

namespace Game
{
    public class Wall : AgentObject
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private float defense;

        public override float MaxHealth => maxHealth;
        public override float Defense => defense;
    }
}