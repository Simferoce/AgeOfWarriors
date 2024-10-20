using Game.Components;
using System;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class KillProjectileEffect : ProjectileEffect, IProjectileStandardEffect, IProjectileImpactEffect
    {
        [SerializeReference, SubclassSelector] private ProjectileDeath projectileDeath;

        public void Execute()
        {
            projectileDeath.Start(projectile, null);
            projectile.Kill(projectileDeath);
        }

        public void Execute(Collider2D collider, Target target)
        {
            projectileDeath.Start(projectile, target.gameObject);
            projectile.Kill(projectileDeath);
        }
    }
}
