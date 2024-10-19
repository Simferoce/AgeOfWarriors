using System;

namespace Game.Ability
{
    [Serializable]
    public class IsEnemyAbilityTargetFilter : AbilityTargetFilter
    {
        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            return source.Faction != targetEntity.Faction;
        }
    }
}
