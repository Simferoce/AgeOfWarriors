using Game.Components;
using Game.Utilities;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class IsGroundProjectileImpactTargetFilter : ProjectileTargetFilter, IImpactProjectileTargetFilter
    {
        public bool Execute(Collider2D collider, Target target)
        {
            return collider.gameObject.CompareTag(GameTag.GROUND);
        }
    }
}