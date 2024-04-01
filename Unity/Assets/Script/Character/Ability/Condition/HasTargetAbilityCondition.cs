using System;

namespace Game
{
    [Serializable]
    public class HasTargetAbilityCondition : AbilityCondition
    {
        public override bool Execute()
        {
            return character.GetTarget() != null;
        }
    }
}
