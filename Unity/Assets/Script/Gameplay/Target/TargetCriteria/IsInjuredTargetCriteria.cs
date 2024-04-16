using System;

namespace Game
{
    [Serializable]
    public class IsInjuredTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable, object caller)
        {
            return (targeteable.TryGetCachedComponent<Character>(out Character character)) && character.IsInjured;
        }
    }
}
