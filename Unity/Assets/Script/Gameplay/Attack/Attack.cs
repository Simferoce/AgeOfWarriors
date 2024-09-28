using System;

namespace Game
{
    public class Attack
    {
        [Flags]
        public enum Flag
        {
            None = 0,
            Ranged = 1 << 0,
            Empowered = 1 << 1,
            OverTime = 1 << 2,
            Reflectable = 1 << 3,
        }

        public AttackFactory Source { get; set; }
        public float Damage { get; set; }
        public float ArmorPenetration { get; set; }
        public float Leach { get; set; }
        public Flag Flags { get; set; }

        public Attack(float damage, float armorPenetration, float leach, Flag flags, AttackFactory source)
        {
            Leach = leach;
            Damage = damage;
            ArmorPenetration = armorPenetration;
            Source = source;
            Flags = flags;
        }
    }
}
