using System;

namespace Game.Modifier
{
    [Serializable]
    public abstract class ModifierParameterFactory
    {
        public abstract void Initialize(object context);
        public abstract ModifierParameter Create(object entity);
    }
}
