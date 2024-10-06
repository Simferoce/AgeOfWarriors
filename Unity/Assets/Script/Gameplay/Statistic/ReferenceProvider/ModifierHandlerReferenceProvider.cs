using System;

namespace Game
{
    [Serializable]
    public class ModifierHandlerReferenceProvider : ReferenceProvider
    {
        public override object Resolve(object context)
        {
            if (!(context is Entity entity))
                throw new ArgumentException();

            return entity.AddOrGetCachedComponent<ModifierHandler>();
        }
    }
}
