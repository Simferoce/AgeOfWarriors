using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class DistanceAbilityTargetFilter : AbilityTargetFilter
    {
        [SerializeField] private StatisticReference<float> distance;
        [SerializeField] private StatisticReference<float> minDistance;

        public float Distance => distance.GetOrThrow()?.Get<float>() ?? 0f;

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
            distance?.Initialize(ability);
            minDistance?.Initialize(ability);
        }

        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            Target target = targetEntity.GetCachedComponent<Target>();
            float targetDistance = Mathf.Abs((target.ClosestPoint(source.Caster.transform.position) - source.Caster.transform.position).x);
            return (!distance.HasValue() || targetDistance < distance.GetOrThrow()) && (!minDistance.HasValue() || targetDistance > minDistance.GetOrThrow());
        }
    }
}
