using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DistanceTargetCriteria : TargetCriteria
    {
        [SerializeField] private StatisticReference<float> reference;

        public override bool Execute(ITargeteable owner, ITargeteable targeteable, object caller, Faction ownerFaction, Faction targetFaction)
        {
            if (!reference.TryGetValue(caller as Ability, out float value))
                throw new Exception($"Could not resolve the reference {reference} from {caller}");

            return Mathf.Abs((targeteable.ClosestPoint(owner.CenterPosition) - owner.CenterPosition).x) < value;
        }
    }
}
