using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class TargetRangeTargetFilter : ModifierTargetFilter
    {
        [SerializeField] private StatisticReference range;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            range.Initialize(modifier);
        }

        public override bool Execute(Entity target)
        {
            if (modifier.Target.Entity == target)
                return false;

            if (Mathf.Abs(modifier.Target.Entity.transform.position.x - target.transform.position.x) > range.Get().Get<float>())
                return false;

            return true;
        }
    }
}