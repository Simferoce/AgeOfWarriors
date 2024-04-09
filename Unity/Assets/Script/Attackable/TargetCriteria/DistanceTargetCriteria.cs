using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DistanceTargetCriteria : TargetCriteria
    {
        [SerializeField] private float reachPercentage;

        public override bool Execute(ITargeteable owner, ITargeteable targeteable)
        {
            return Mathf.Abs((targeteable.ClosestPoint(owner.CenterPosition) - owner.CenterPosition).x) < (owner as AgentObject).Reach * reachPercentage;
        }

        public override TargetCriteria Clone()
        {
            DistanceTargetCriteria criteria = new DistanceTargetCriteria();
            criteria.reachPercentage = reachPercentage;
            return criteria;
        }
    }
}
