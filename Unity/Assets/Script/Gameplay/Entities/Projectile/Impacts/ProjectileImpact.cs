﻿using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public abstract class ProjectileImpact
    {
        protected ProjectileEntity projectile;

        public virtual void Initialize(ProjectileEntity projectile)
        {
            this.projectile = projectile;
        }

        public virtual bool Validate(ProjectileEntity projectile) { return false; }
        public abstract void Impact(Collider2D collider);
        public virtual void LeaveZone(Collider2D collider) { }
        public virtual void Update() { }
    }
}
