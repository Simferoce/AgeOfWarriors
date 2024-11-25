using Game.Agent;
using System;

namespace Game.Modifier
{
    [Serializable]
    public class TargetEnemyModifierTargetFilter : ModifierTargetFilter
    {
        public override bool Execute(Entity target)
        {
            return modifier.Target.Entity.TryGetCachedComponent<AgentIdentity>(out AgentIdentity selfIdentity)
                && target.TryGetCachedComponent<AgentIdentity>(out AgentIdentity targetIdentity)
                && selfIdentity.Faction != targetIdentity.Faction;
        }
    }
}
