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

        public void Execute(Entity entity)
        {
            projectileDeath.Start(projectile, entity.gameObject);
            projectile.Kill(projectileDeath);
        }
    }
}
