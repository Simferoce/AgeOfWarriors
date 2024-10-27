using System;

namespace Game.Modifier
{
    [Serializable]
    public abstract class ModifierParameterFactory
    {
        public virtual void Initialize(Entity entity) { }
        public abstract ModifierParameter Create(object entity);
    }
}
