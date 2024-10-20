using Game.Agent;
using Game.Components;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class IsAllyProjectileImpactFilter : ProjectileImpactFilter
    {
        public override bool Execute(Collider2D collider, Target target)
        {
            return target != null && (target.Entity is AgentObject agentObject) && agentObject.Faction == projectile.Faction;
        }
    }
}
