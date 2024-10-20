using Game.Components;
using UnityEngine;

namespace Game.Projectile
{
    public interface IProjectileImpactEffect
    {
        public void Execute(Collider2D collider, Target target);
    }
}
