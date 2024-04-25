using System.Collections.Generic;

namespace Game
{
    public interface IModifierSource
    {
        public event System.Action<Modifier> OnModifierAdded;

        public List<Modifier> AppliedModifiers { get; }

        public void AddModifier(Modifier modifier);
    }
}
