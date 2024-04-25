using System.Collections.Generic;

namespace Game
{
    public interface IModifierSource
    {
        public List<Modifier> AppliedModifiers { get; }
    }
}
