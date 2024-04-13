using System;
using System.Collections.Generic;

namespace Game
{
    public class ShieldHandler
    {
        public event IShieldable.ShieldBroken OnShieldBroken;
        public event Action<IShieldable> OnDestroyed;

        private List<Shield> shields = new List<Shield>();
        public List<Shield> Shields { get => shields; set => shields = value; }

        private IShieldable owner;

        public void Initialize(IShieldable owner)
        {
            this.owner = owner;
        }

        public void AddShield(Shield shield)
        {
            shields.Add(shield);
        }

        public void Update()
        {
            for (int i = shields.Count - 1; i >= 0; i--)
            {
                Shield shield = shields[i];
                if (shield.Update())
                {
                    shields.Remove(shield);
                    OnShieldBroken?.Invoke(shield);
                }
            }
        }

        public float Absorb(float damageRemaining)
        {
            for (int i = shields.Count - 1; i >= 0; i--)
            {
                Shield shield = shields[i];
                if (!shield.Absorb(damageRemaining, out damageRemaining))
                {
                    OnShieldBroken?.Invoke(shield);
                    shields.RemoveAt(i);
                }
            }

            return damageRemaining;
        }

        public void OnDestroy()
        {
            OnDestroyed?.Invoke(owner);
        }
    }

}
