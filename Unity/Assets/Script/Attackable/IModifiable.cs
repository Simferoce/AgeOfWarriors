using System.Collections.Generic;

namespace Game
{
    public interface IModifiable : ITargeteable
    {
        public void AddModifier(Modifier modifier);
        public void RemoveModifier(Modifier modifier);
        public List<Modifier> GetModifiers();
    }
}
