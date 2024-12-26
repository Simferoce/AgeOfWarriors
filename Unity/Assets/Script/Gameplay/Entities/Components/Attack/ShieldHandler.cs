using System.Collections.Generic;
using System.Linq;

namespace Game.Components
{
    public class ShieldHandler
    {
        private List<IShield> shields = new List<IShield>();

        public delegate void OnShieldRemovedDelegate(IShield shield);
        public event OnShieldRemovedDelegate OnShieldRemoved;

        public delegate void OnAbsorbedDelegate(float amount);
        public event OnAbsorbedDelegate OnAbsorbed;

        public float Remaining => shields.Sum(x => x.Remaining);

        public float Absorb(float damage)
        {
            float absorbedDamage = 0f;
            foreach (IShield shield in shields)
            {
                float before = damage;
                damage = shield.Absorb(damage);

                absorbedDamage += before - damage;

                if (damage == 0)
                    break;
            }

            if (absorbedDamage > 0)
                OnAbsorbed?.Invoke(absorbedDamage);

            return damage;
        }

        public void Add(IShield shield)
        {
            shields.Add(shield);
        }

        public void Remove(IShield shield)
        {
            shields.Remove(shield);
            OnShieldRemoved?.Invoke(shield);
        }
    }
}
