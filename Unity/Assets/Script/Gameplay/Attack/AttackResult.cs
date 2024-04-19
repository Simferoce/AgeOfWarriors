namespace Game
{
    public class AttackResult
    {
        public Attack Attack { get; set; }
        public float DamageTaken { get; set; }
        public float DefenseDamagePrevented { get; set; }
        public bool KillingBlow { get; set; }
        public IAttackable Target { get; set; }

        public AttackResult(Attack attack, float damageTaken, float defenseDamagePrevented, bool killingBlow, IAttackable target)
        {
            Attack = attack;
            DamageTaken = damageTaken;
            DefenseDamagePrevented = defenseDamagePrevented;
            KillingBlow = killingBlow;
            Target = target;
        }
    }
}
