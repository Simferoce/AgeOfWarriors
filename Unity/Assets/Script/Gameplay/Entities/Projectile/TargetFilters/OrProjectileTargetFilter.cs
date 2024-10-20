using Game.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class OrProjectileTargetFilter : ProjectileTargetFilter, IStandardProjectileTargetFilter, IImpactProjectileTargetFilter
    {
        [SerializeReference, SubclassSelector] private List<ProjectileTargetFilter> filters;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);

            foreach (ProjectileTargetFilter filter in filters)
                filter.Initialize(projectile);
        }

        public bool Execute(Collider2D collider, Target target)
        {
            foreach (ProjectileTargetFilter filter in filters)
            {
                if ((filter is IImpactProjectileTargetFilter impactTargetFilter && impactTargetFilter.Execute(collider, target))
                    || (filter is IStandardProjectileTargetFilter standardEffect && standardEffect.Execute(target)))
                    return true;
            }

            return false;
        }

        public bool Execute(Target target)
        {
            foreach (ProjectileTargetFilter filter in filters)
            {
                if (filter is IStandardProjectileTargetFilter standardEffect && standardEffect.Execute(target))
                    return true;
            }

            return false;
        }
    }
}