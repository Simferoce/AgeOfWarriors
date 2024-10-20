using Game.Components;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class IsGoingDownProjectileImpactFilter : ProjectileImpactFilter
    {
        public override bool Execute(Collider2D collider, Target target)
        {
            return projectile.Rigidbody.linearVelocity.y < 0;
        }
    }
}