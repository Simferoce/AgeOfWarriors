using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class AndTargetFilter : ModifierTargetFilter
    {
        [SerializeReference, SubclassSelector] private List<ModifierTargetFilter> targetFilters;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            foreach (ModifierTargetFilter targetFilter in targetFilters)
                targetFilter.Initialize(modifier);
        }

        public override bool Execute(Entity target)
        {
            return targetFilters.Aggregate(true, (a, b) => a && b.Execute(target));
        }
    }
}