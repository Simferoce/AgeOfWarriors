using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ProjectileAttackData : ProjectileData
    {
        public class Context : ProjectileContext
        {
            public Attack Attack { get; set; }

            public override void Initialize(Projectile projectile)
            {
                base.Initialize(projectile);
                Attack.AttackSource.Sources.Add(projectile);
            }
        }

        [SerializeField, Range(0, 3)] private float attackPowerPercentage;
        [SerializeField] private float armorPenetration;

        public override ProjectileContext GetContext(Character character)
        {
            AttackSource attackSource = new AttackSource(character);
            Attack attack = new Attack(attackSource, character.AttackPower * attackPowerPercentage, armorPenetration, 0f);
            return new Context() { Attack = attack };
        }
    }
}
