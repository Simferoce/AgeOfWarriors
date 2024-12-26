using System.Collections.Generic;
using System.Linq;

namespace Game.Components
{
    public class ShieldHandler
    {
        private List<IShield> shields = new List<IShield>();

        public delegate void OnShieldRemovedDelegate(IShield shield);
        public event OnShieldRemovedDelegate OnShieldRemoved;

        public float Remaining => shields.Sum(x => x.Remaining);

        public float Absorb(float damage)
        {
            foreach (IShield shield in shields)
            {
                damage = shield.Absorb(damage);

                if (damage == 0)
                    return 0;
            }

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
