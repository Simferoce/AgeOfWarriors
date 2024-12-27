using Game.Pool;
using System;

namespace Game.Modifier
{
    [Serializable]
    public class IsPoolTargetFilter : ModifierTargetFilter
    {
        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
        }

        public override bool Execute(Entity target)
        {
            return target is PoolEntity;
        }
    }
}