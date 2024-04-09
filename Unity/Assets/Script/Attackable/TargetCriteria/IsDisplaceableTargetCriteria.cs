using System;

namespace Game
{
    [Serializable]
    public class IsDisplaceableTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable)
        {
            return targeteable.IsDisplaceable();
        }

        public override TargetCriteria Clone()
        {
            return new IsDisplaceableTargetCriteria();
        }
    }
}
