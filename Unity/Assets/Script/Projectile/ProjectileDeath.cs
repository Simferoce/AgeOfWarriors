using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class ProjectileDeath
    {
        public abstract void Start(Projectile projectile, GameObject collision);
        public virtual void Update(Projectile projectile) { }
    }
}
