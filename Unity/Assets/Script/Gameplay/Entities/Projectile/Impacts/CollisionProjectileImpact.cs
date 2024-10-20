using Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class CollisionProjectileImpact : ProjectileImpact
    {
        [SerializeReference, SubclassSelector] private ProjectileTargetFilter filter;
        [SerializeReference, SubclassSelector] private List<ProjectileEffect> effects;

        private List<Collider2D> collidersProcessed = new List<Collider2D>();

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);

            if (filter != null)
                filter.Initialize(projectile);

            foreach (ProjectileEffect effect in effects)
                effect.Initialize(projectile);
        }

        public override bool Validate(ProjectileEntity projectile)
        {
            bool changed = base.Validate(projectile);
            if (filter != null)
                changed |= filter.Validate(projectile);
            foreach (ProjectileEffect effect in effects.Where(x => x != null))
                changed |= effect.Validate(projectile);

            return changed;
        }

        public override void Impact(Collider2D collider)
        {
            if (collidersProcessed.Contains(collider))
                return;

            Target target = collider.gameObject.GetComponentInParent<Target>();
            if (target != null && !target.Entity.IsActive)
                return;

            if (filter is IImpactProjectileTargetFilter impactImpactTargetFilter && !impactImpactTargetFilter.Execute(collider, target))
                return;

            if (filter is IStandardProjectileTargetFilter standardProjectileTargetFilter && !standardProjectileTargetFilter.Execute(target))
                return;

            foreach (IProjectileImpactEffect effect in effects.OfType<IProjectileImpactEffect>())
                effect.Execute(target.Entity);

            foreach (IProjectileStandardEffect effect in effects.OfType<IProjectileStandardEffect>().Where(x => x is not IProjectileImpactEffect))
                effect.Execute();

            collidersProcessed.Add(collider);
        }
    }
}
