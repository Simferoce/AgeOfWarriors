using System;

namespace Game.Ability
{
    [Serializable]
    public class IsAllyAbilityTargetFilter : AbilityTargetFilter
    {
        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            return source.Faction == targetEntity.Faction;
        }
    }
}
