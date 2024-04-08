using System;

namespace Game
{
    [Serializable]
    public class IsInjuredTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable, StatisticContext context)
        {
            return targeteable.IsInjured();
        }

        public override TargetCriteria Clone()
        {
            return new IsInjuredTargetCriteria();
        }
    }
}
