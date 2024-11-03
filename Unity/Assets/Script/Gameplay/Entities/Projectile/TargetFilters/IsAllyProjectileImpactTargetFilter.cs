using Game.Agent;
using Game.Components;
using System;

namespace Game.Projectile
{
    [Serializable]
    public class IsAllyProjectileImpactTargetFilter : ProjectileTargetFilter, IStandardProjectileTargetFilter
    {
        public bool Execute(Target target)
        {
            return target != null && target.Entity.TryGetCachedComponent<AgentIdentity>(out AgentIdentity agentIdentity) && agentIdentity.Faction == projectile.Faction;
        }
    }
}
