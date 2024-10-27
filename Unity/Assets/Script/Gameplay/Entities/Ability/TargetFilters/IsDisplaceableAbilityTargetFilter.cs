using System;

namespace Game.Ability
{
    [Serializable]
    public class IsDisplaceableAbilityTargetFilter : AbilityTargetFilter
    {
        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            return !targetEntity.Tags.Contains(Entity.EntityTag.Building);
        }
    }
}
