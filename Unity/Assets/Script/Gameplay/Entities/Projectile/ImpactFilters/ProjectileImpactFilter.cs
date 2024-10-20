using Game.Components;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public abstract class ProjectileImpactFilter
    {
        protected ProjectileEntity projectile;

        public virtual void Initialize(ProjectileEntity projectile)
        {
            this.projectile = projectile;
        }

        public abstract bool Execute(Collider2D collider, Target target);
    }
}
