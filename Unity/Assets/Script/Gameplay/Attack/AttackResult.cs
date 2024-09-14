using UnityEngine;

namespace Game
{
    public class AttackResult
    {
        public Attack Attack { get; set; }
        public float DamageTaken { get; set; }
        public float DefenseDamagePrevented { get; set; }
        public bool KillingBlow { get; set; }
        public Attackable Target { get; set; }

        public AttackResult(Attack attack, float damageTaken, float defenseDamagePrevented, bool killingBlow, Attackable target)
        {
            Attack = attack;
            DamageTaken = damageTaken;
            DefenseDamagePrevented = defenseDamagePrevented;
            KillingBlow = killingBlow;
            Target = target;

            Debug.Log($"{target} is taking {damageTaken} (reduced by {defenseDamagePrevented}) from {attack.AttackSource.Sources[^1]}");
        }
    }
}
