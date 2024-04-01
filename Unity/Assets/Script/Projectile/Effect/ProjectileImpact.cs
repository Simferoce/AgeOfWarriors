using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class ProjectileImpact
    {
        public abstract bool Impact(GameObject collision, Projectile projectile);
    }
}
