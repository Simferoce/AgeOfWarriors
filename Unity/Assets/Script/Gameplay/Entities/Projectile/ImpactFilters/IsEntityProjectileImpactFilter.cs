using Game.Components;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class IsEntityProjectileImpactFilter : ProjectileImpactFilter
    {
        public override bool Execute(Collider2D collider, Target target)
        {
            return target != null;
        }
    }
}
