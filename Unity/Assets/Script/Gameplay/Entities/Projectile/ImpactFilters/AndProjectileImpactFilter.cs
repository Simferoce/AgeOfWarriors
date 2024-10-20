using Game.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class AndProjectileImpactFilter : ProjectileImpactFilter
    {
        [SerializeReference, SubclassSelector] private List<ProjectileImpactFilter> filters;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);

            foreach (ProjectileImpactFilter filter in filters)
                filter.Initialize(projectile);
        }

        public override bool Execute(Collider2D collider, Target target)
        {
            foreach (ProjectileImpactFilter filter in filters)
            {
                if (!filter.Execute(collider, target))
                    return false;
            }

            return true;
        }
    }
}
