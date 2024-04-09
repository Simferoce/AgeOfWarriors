using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileFactoryHealContext : ProjectileFactoryContext
    {
        public class Context : ProjectileContext
        {
            public float HealAmount { get; set; }
        }

        [SerializeField, Range(0, 3)] private float healPercentageAttackPower = 1f;

        public override ProjectileContext GetContext(Character character)
        {
            return new Context() { HealAmount = character.AttackPower * healPercentageAttackPower };
        }
    }
}
