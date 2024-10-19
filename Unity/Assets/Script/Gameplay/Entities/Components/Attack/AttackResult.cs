using UnityEngine;

namespace Game.Components
{
    public class AttackResult
    {
        public AttackData AttackData { get; set; }
        public float DamageTaken { get; set; }
        public float DefenseDamagePrevented { get; set; }
        public bool KillingBlow { get; set; }
        public Attackable Target { get; set; }
        public bool ResistedKillingBlow { get; set; }

        public AttackResult(AttackData attack, float damageTaken, float defenseDamagePrevented, bool killingBlow, Attackable target, bool resistedKillingBlow)
        {
            AttackData = attack;
            DamageTaken = damageTaken;
            DefenseDamagePrevented = defenseDamagePrevented;
            KillingBlow = killingBlow;
            Target = target;
            ResistedKillingBlow = resistedKillingBlow;

            Debug.Log($"{target} is taking {damageTaken} (reduced by {defenseDamagePrevented}) from {attack.Source}");
        }
    }
}
