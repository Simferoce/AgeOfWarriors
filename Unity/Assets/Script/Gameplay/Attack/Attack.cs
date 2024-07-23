namespace Game
{
    public class Attack
    {
        public AttackSource AttackSource { get; set; }
        public float Damage { get; set; }
        public float ArmorPenetration { get; set; }
        public float Leach { get; set; }
        public bool Ranged { get; set; }
        public bool Empowered { get; set; }
        public bool OverTime { get; set; }
        public bool Reflectable { get; set; }

        public Attack(AttackSource attackSource, float damage, float armorPenetration, float leach, bool ranged, bool empowered, bool reflectable, bool overTime)
        {
            Reflectable = reflectable;
            Leach = leach;
            AttackSource = attackSource;
            Damage = damage;
            ArmorPenetration = armorPenetration;
            Empowered = empowered;
            OverTime = overTime;
            Ranged = ranged;
        }

        public Attack Clone()
        {
            return new Attack(AttackSource.Clone(), Damage, ArmorPenetration, Leach, Ranged, Empowered, Reflectable, OverTime);
        }
    }
}
