using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public abstract class ProjectileDeath
    {
        public abstract void Start(ProjectileEntity projectile, GameObject collision);
        public virtual void Update(ProjectileEntity projectile) { }
    }
}
