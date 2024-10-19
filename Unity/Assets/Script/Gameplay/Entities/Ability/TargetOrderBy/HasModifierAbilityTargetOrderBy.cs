using Game.Components;
using Game.Modifier;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class HasModifierAbilityTargetOrderBy : AbilityTargetOrderBy
    {
        [SerializeField] private ModifierDefinition modifierDefinition;

        public override IOrderedEnumerable<Target> OrderBy(IEnumerable<Target> targets)
        {
            if (targets is IOrderedEnumerable<Target> orderedTargets)
                return orderedTargets.ThenByDescending(x => x.Entity.TryGetCachedComponent<ModifierHandler>(out ModifierHandler handler) && handler.GetModifier(modifierDefinition) != null);

            return targets.OrderByDescending(x => x.Entity.TryGetCachedComponent<ModifierHandler>(out ModifierHandler handler) && handler.GetModifier(modifierDefinition) != null);
        }
    }
}
