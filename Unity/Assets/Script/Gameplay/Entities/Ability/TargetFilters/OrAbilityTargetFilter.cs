using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class OrAbilityTargetFilter : AbilityTargetFilter
    {
        [SerializeReference, SubclassSelector] private List<AbilityTargetFilter> filters = new List<AbilityTargetFilter>();

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
            foreach (AbilityTargetFilter filter in filters.Where(x => x != null))
                filter.Initialize(ability);
        }

        public override bool Validate()
        {
            bool changed = base.Validate();
            foreach (AbilityTargetFilter filter in filters)
            {
                changed |= filter.Validate();
            }

            return changed;
        }

        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            if (filters.Count == 0)
                return true;

            foreach (AbilityTargetFilter filter in filters)
            {
                if (filter.Execute(source, targetEntity))
                    return true;
            }

            return false;
        }
    }
}
