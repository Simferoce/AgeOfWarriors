using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DistanceTargetCriteria : TargetCriteria
    {
        [SerializeField] private StatisticReference<float> reference;

        public override bool Execute(ITargeteable owner, ITargeteable targeteable, IStatisticProviderOld statisticProvider, Faction ownerFaction, Faction targetFaction)
        {
            float value = reference.GetValueOrThrow(statisticProvider);

            return Mathf.Abs((targeteable.ClosestPoint(owner.CenterPosition) - owner.CenterPosition).x) < value;
        }
    }
}
