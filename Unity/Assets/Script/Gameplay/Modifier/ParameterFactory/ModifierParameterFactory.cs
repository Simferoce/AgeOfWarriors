using System;

namespace Game
{
    [Serializable]
    public abstract class ModifierParameterFactory
    {
        public abstract ModifierParameter Create(Modifier modifier);
    }
}
