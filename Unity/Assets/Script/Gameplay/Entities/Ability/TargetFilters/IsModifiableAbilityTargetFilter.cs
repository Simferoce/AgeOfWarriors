using Game.Modifier;
using System;

namespace Game.Ability
{
    [Serializable]
    public class IsModifiableAbilityTargetFilter : AbilityTargetFilter
    {
        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            return targetEntity.TryGetCachedComponent<ModifierHandler>(out _);
        }
    }
}
