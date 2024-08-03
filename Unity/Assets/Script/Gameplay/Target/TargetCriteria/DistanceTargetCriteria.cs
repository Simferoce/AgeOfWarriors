using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DistanceTargetCriteria : TargetCriteria
    {
        [SerializeField] private StatisticReference<float> reference;

        public override bool Execute(ITargeteable owner, ITargeteable targeteable, Context context, Faction ownerFaction, Faction targetFaction)
        {
            if (!context.TryGetContextElement<AbilityCastingContextElement>(out AbilityCastingContextElement abilityCastingContext))
                throw new Exception($"Expecting the context of the call to be done with {nameof(AbilityCastingContextElement)}");

            if (!reference.TryGetValue(abilityCastingContext.Ability, out float value))
                throw new Exception($"Could not resolve the reference {reference} from {abilityCastingContext.Ability.Definition.Name}");

            return Mathf.Abs((targeteable.ClosestPoint(owner.CenterPosition) - owner.CenterPosition).x) < value;
        }
    }
}
