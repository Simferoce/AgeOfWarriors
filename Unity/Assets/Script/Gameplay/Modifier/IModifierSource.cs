using System.Collections.Generic;

namespace Game
{
    public interface IModifierSource : IComponent
    {
        public event System.Action<Modifier> OnModifierAdded;

        public List<Modifier> AppliedModifiers { get; }

        public void AddAppliedModifier(Modifier modifier);
        public void RemoveAppliedModifier(Modifier modifier);
    }
}
