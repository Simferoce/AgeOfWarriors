using System;

namespace Game
{
    [Serializable]
    public class IsDisplaceableTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable, StatisticContext context)
        {
            return targeteable.IsDisplaceable();
        }

        public override TargetCriteria Clone()
        {
            return new IsDisplaceableTargetCriteria();
        }
    }
}
