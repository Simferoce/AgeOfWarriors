using Game.Agent;
using Game.Components;
using System;

namespace Game.Projectile
{
    [Serializable]
    public class IsEnemyProjectileImpactTargetFilter : ProjectileTargetFilter, IStandardProjectileTargetFilter
    {
        public bool Execute(Target target)
        {
            return target != null && (target.Entity is AgentObject agentObject) && agentObject.Faction != projectile.Faction;
        }
    }
}
