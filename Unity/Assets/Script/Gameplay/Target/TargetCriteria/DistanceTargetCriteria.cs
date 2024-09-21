using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DistanceTargetCriteria : TargetCriteria
    {
        [SerializeField] private StatisticReference reference;

        public override bool Execute(Target owner, Target targeteable, IStatisticContext statisticProvider, Faction ownerFaction, Faction targetFaction)
        {
            float value = reference.GetValueOrThrow<float>(statisticProvider);

            return Mathf.Abs((targeteable.ClosestPoint(owner.CenterPosition) - owner.CenterPosition).x) < value;
        }
    }
}
