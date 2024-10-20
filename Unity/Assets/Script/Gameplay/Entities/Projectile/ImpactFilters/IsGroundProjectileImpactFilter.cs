using Game.Components;
using Game.Utilities;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class IsGroundProjectileImpactFilter : ProjectileImpactFilter
    {
        public override bool Execute(Collider2D collider, Target target)
        {
            return collider.gameObject.CompareTag(GameTag.GROUND);
        }
    }
}