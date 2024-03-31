namespace Game
{
    public class Attack
    {
        public AttackSource AttackSource { get; set; }
        public float Damage { get; set; }
        public float ArmorPenetration { get; set; }
        public float Leach { get; set; }

        public Attack(AttackSource attackSource, float damage, float armorPenetration)
        {
            AttackSource = attackSource;
            Damage = damage;
            ArmorPenetration = armorPenetration;
        }

        public Attack Clone()
        {
            return new Attack(AttackSource.Clone(), Damage, ArmorPenetration);
        }
    }
}
