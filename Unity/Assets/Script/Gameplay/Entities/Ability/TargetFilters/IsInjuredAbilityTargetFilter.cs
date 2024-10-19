using Game.Character;
using System;

namespace Game.Ability
{
    [Serializable]
    public class IsInjuredAbilityTargetFilter : AbilityTargetFilter
    {
        public override bool Execute(AbilityEntity source, Entity targetEntity)
        {
            return (targetEntity is CharacterEntity character) && character.IsInjured;
        }
    }
}
