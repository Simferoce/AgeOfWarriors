using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class ProjectileMovement
    {
        protected Projectile projectile;

        public virtual void Initialize(Projectile projectile, Vector3 target)
        {
            this.projectile = projectile;
        }

        public abstract void Update();
    }
}
