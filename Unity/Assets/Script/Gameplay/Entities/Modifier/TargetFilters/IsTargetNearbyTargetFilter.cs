using Game.Components;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class IsTargetNearbyTargetFilter : ModifierTargetFilter
    {
        [SerializeField] private StatisticReference range;
        [SerializeReference, SubclassSelector] private ModifierTargetFilter targetFilter;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            range.Initialize(modifier);
            targetFilter.Initialize(modifier);
        }

        public override bool Execute(Entity target)
        {
            foreach (Target potentialTarget in Target.All)
            {
                if (potentialTarget.Entity == target)
                    continue;

                if (Mathf.Abs(modifier.Target.Entity.transform.position.x - potentialTarget.CenterPosition.x) > range.Get().Get<float>())
                    continue;

                if (!targetFilter.Execute(potentialTarget.Entity))
                    continue;

                return true;
            }

            return false;
        }
    }
}