using System;

namespace Game.Modifier
{
    [Serializable]
    public abstract class ModifierParameterFactory
    {
        public abstract ModifierParameter Create(ModifierEntity modifier);
    }
}
