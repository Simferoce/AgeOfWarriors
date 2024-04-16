using System.Collections.Generic;

namespace Game
{
    public interface IShieldable : IComponent
    {
        delegate void ShieldBroken(Shield shield);
        public event System.Action<IShieldable> OnShieldableDestroyed;
        public List<Shield> Shields { get; }

        public event ShieldBroken OnShieldBroken;
        public void AddShield(Shield shield);
    }
}
