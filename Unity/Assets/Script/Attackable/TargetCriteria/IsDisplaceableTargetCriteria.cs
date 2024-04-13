using System;

namespace Game
{
    [Serializable]
    public class IsDisplaceableTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable, object caller)
        {
            return (targeteable is IDisplaceable displaceable) && displaceable.IsDisplaceable;
        }
    }
}
