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
        public bool ResistedKillingBlow { get; set; }

        public AttackResult(Attack attack, float damageTaken, float defenseDamagePrevented, bool killingBlow, Attackable target, bool resistedKillingBlow)
        {
            Attack = attack;
            DamageTaken = damageTaken;
            DefenseDamagePrevented = defenseDamagePrevented;
            KillingBlow = killingBlow;
            Target = target;
            ResistedKillingBlow = resistedKillingBlow;

            Debug.Log($"{target} is taking {damageTaken} (reduced by {defenseDamagePrevented}) from {attack.Source}");
        }
    }
}
