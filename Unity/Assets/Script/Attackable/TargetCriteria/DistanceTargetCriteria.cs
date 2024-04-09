using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DistanceTargetCriteria : TargetCriteria
    {
        [SerializeReference, SerializeReferenceDropdown] private IStatisticFloat distance;

        public override bool Execute(ITargeteable owner, ITargeteable targeteable, StatisticContext context)
        {
            return Mathf.Abs((targeteable.ClosestPoint(owner.CenterPosition) - owner.CenterPosition).x) < distance.GetValue(context);
        }
    }
}
