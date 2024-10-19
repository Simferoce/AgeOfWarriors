using Game.Agent;
using Game.Character;
using System;

namespace Game.Ability
{
    [Serializable]
    public class IsDisplaceableAbilityTargetFilter : AbilityTargetFilter
    {
        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            return (targetEntity is CharacterEntity) && (targetEntity as AgentObject).IsActive;
        }
    }
}
