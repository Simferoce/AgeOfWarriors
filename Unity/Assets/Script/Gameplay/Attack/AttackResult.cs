namespace Game
{
    public class AttackResult
    {
        public Attack Attack { get; set; }
        public float DamageTaken { get; set; }
        public float DefenseDamagePrevented { get; set; }

        public AttackResult(Attack attack, float damageTaken, float defenseDamagePrevented)
        {
            Attack = attack;
            DamageTaken = damageTaken;
            DefenseDamagePrevented = defenseDamagePrevented;
        }
    }
}
