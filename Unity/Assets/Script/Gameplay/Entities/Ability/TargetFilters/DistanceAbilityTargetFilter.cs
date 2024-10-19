using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class DistanceAbilityTargetFilter : AbilityTargetFilter
    {
        [SerializeReference, SubclassSelector] private AbilityStatistic distance;

        public override bool Validate()
        {
            bool changed = base.Validate();
            if (distance != null && distance.Definition != StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Range))
            {
                distance.Definition = StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Range);
                changed = true;
            }

            return changed;
        }

        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            Target target = targetEntity.GetCachedComponent<Target>();
            return Mathf.Abs((target.ClosestPoint(source.Caster.transform.position) - source.Caster.transform.position).x) < distance.GetValue<float>(source);
        }
    }
}
