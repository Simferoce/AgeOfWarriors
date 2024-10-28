using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class DistanceAbilityTargetFilter : AbilityTargetFilter
    {
        [SerializeReference, SubclassSelector] private Value distance;

        public float Distance => distance?.GetValue<float>() ?? 0f;

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
            distance.Initialize(ability);
        }

        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            Target target = targetEntity.GetCachedComponent<Target>();
            return Mathf.Abs((target.ClosestPoint(source.Caster.transform.position) - source.Caster.transform.position).x) < Distance;
        }
    }
}
