namespace Game
{
    public class Attack
    {
        public AttackFactory AttackFactory { get; set; }
        public float Damage { get; set; }
        public float ArmorPenetration { get; set; }
        public float Leach { get; set; }
        public bool Ranged { get; set; }
        public bool Empowered { get; set; }
        public bool OverTime { get; set; }
        public bool Reflectable { get; set; }

        public Attack(AttackFactory attackFactory, float damage, float armorPenetration, float leach, bool ranged, bool empowered, bool reflectable, bool overTime)
        {
            Reflectable = reflectable;
            Leach = leach;
            AttackFactory = attackFactory;
            Damage = damage;
            ArmorPenetration = armorPenetration;
            Empowered = empowered;
            OverTime = overTime;
            Ranged = ranged;
        }
    }
}
