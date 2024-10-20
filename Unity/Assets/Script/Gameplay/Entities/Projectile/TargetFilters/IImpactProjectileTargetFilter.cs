using Game.Components;
using UnityEngine;

namespace Game.Projectile
{
    public interface IImpactProjectileTargetFilter
    {
        public bool Execute(Collider2D collider, Target target);
    }
}
