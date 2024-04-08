using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileHealData : ProjectileData
    {
        public class Context : ProjectileContext
        {
            public float HealAmount { get; set; }
        }

        [SerializeField, Range(0, 3)] private float healPercentageAttackPower = 1f;

        public override ProjectileContext GetContext(Character character)
        {
            return new Context() { HealAmount = character.AttackPower.GetValue() * healPercentageAttackPower };
        }

        public override ProjectileData Clone()
        {
            ProjectileHealData data = new ProjectileHealData();
            data.healPercentageAttackPower = healPercentageAttackPower;
            return data;
        }
    }
}
