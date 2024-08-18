using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DistanceTargetCriteria : TargetCriteria
    {
        [SerializeField] private StatisticReference<float> reference;

        public override bool Execute(ITargeteable owner, ITargeteable targeteable, IContext context, Faction ownerFaction, Faction targetFaction)
        {
            float value = reference.GetValueOrThrow(context);

            return Mathf.Abs((targeteable.ClosestPoint(owner.CenterPosition) - owner.CenterPosition).x) < value;
        }
    }
}
