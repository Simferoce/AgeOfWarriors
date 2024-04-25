using System.Collections.Generic;

namespace Game
{
    public interface IModifiable : IComponent
    {
        public event System.Action<Modifier> OnModifierRemoved;
        public event System.Action<Modifier> OnModifierAdded;

        public void AddModifier(Modifier modifier);
        public void RemoveModifier(Modifier modifier);
        public List<Modifier> GetModifiers();
        public bool TryGetModifier(ModifierDefinition definition, out Modifier modifier);
        public void Clear();
    }
}
