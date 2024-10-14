using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AbilityTargetCriteriaDistance : AbilityTargetCriteria
    {
        [SerializeReference, SubclassSelector] private Statistic distance;

        public override bool Execute(Ability source, Entity targetEntity)
        {
            Target target = targetEntity.GetCachedComponent<Target>();
            return Mathf.Abs((target.ClosestPoint(source.Caster.transform.position) - source.Caster.transform.position).x) < distance.GetValue<float>(source);
        }
    }
}
