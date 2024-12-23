using Game.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class RadiusModifierTarget : ModifierTarget
    {
        [SerializeReference, SubclassSelector] private ModifierTarget center;
        [SerializeReference, SubclassSelector] private ModifierTargetFilter filter;
        [SerializeField] private StatisticReference<float> range;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            center.Initialize(modifier);
            filter.Initialize(modifier);
            range.Initialize(modifier);
        }

        public override List<object> GetTargets()
        {
            Vector3 position = (center.GetTargets()[0] as Entity).transform.position;

            List<object> targets = new List<object>();
            foreach (Entity entity in Entity.All.Where(x => x.IsActive))
            {
                if (filter != null && !filter.Execute(entity))
                    continue;

                if (Mathf.Abs(position.x - entity.transform.position.x) < range.Get().Get<float>())
                {
                    targets.Add(entity);
                }
            }

            return targets;
        }
    }
}
