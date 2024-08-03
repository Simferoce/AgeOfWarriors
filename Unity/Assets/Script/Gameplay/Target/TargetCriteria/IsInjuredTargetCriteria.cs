using System;

namespace Game
{
    [Serializable]
    public class IsInjuredTargetCriteria : TargetCriteria
    {
        public override bool Execute(ITargeteable owner, ITargeteable targeteable, Context context, Faction ownerFaction, Faction targetFaction)
        {
            return (targeteable.TryGetCachedComponent<Character>(out Character character)) && character.IsInjured;
        }
    }
}
