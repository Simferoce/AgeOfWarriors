using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class DistanceTargetCriteria : TargetCriteria
    {
        [SerializeReference, SubclassSelector] private ReferenceProvider reference;
        [SerializeField] private StatisticReference distance;

        public override bool Execute(Entity source, Entity targetEntity)
        {
            distance.Initialize(source);
            float value = distance;
            Transform from = (reference.Resolve(source) as MonoBehaviour).transform;

            Target target = targetEntity.GetCachedComponent<Target>();
            return Mathf.Abs((target.ClosestPoint(from.transform.position) - from.transform.position).x) < value;
        }
    }
}
