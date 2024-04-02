using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class ProjectileImpact
    {
        protected Projectile projectile;

        public virtual void Initialize(Projectile projectile)
        {
            this.projectile = projectile;
        }

        public abstract bool Impact(GameObject collision);
        public virtual void LeaveZone(GameObject collision) { }
        public virtual bool Update() { return false; }
    }
}
