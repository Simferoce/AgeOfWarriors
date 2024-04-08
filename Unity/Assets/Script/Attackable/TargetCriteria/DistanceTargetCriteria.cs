using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DistanceTargetCriteria : TargetCriteria
    {
        [SerializeReference, SubclassSelector] private IStatisticFloat distance;

        public override bool Execute(ITargeteable owner, ITargeteable targeteable, StatisticContext context)
        {
            return Mathf.Abs((targeteable.ClosestPoint(owner.CenterPosition) - owner.CenterPosition).x) < distance.GetValue(context);
        }

        public override TargetCriteria Clone()
        {
            DistanceTargetCriteria criteria = new DistanceTargetCriteria();
            criteria.distance = (IStatisticFloat)distance.Clone();
            return criteria;
        }
    }
}
