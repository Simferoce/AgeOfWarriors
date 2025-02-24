﻿using Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class CollisionProjectileBehaviour : ProjectileBehaviour, IProjectileZoneBehaviour
    {
        [SerializeReference, SubclassSelector] private ProjectileTargetFilter filter;
        [SerializeReference, SubclassSelector] private List<ProjectileEffect> effects;

        private List<Entity> processedEntities = new List<Entity>();

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);

            if (filter != null)
                filter.Initialize(projectile);

            foreach (ProjectileEffect effect in effects)
                effect.Initialize(projectile);
        }

        public void EnterZone(Collider2D collider)
        {
            Target target = collider.gameObject.GetComponentInParent<Target>();
            if (target == null || !target.enabled)
                return;

            if (processedEntities.Contains(target.Entity))
                return;

            ProjectileParameter<Target> projectileParameter = projectile.Parameters.FirstOrDefault(x => x.Name == "ignore") as ProjectileParameter<Target>;
            if (projectileParameter != null && projectileParameter.GetValue() == target)
                return;

            if (filter is IImpactProjectileTargetFilter impactImpactTargetFilter && !impactImpactTargetFilter.Execute(collider, target))
                return;

            if (filter is IStandardProjectileTargetFilter standardProjectileTargetFilter && !standardProjectileTargetFilter.Execute(target))
                return;

            foreach (IProjectileImpactEffect effect in effects.OfType<IProjectileImpactEffect>())
                effect.Execute(target.Entity);

            foreach (IProjectileStandardEffect effect in effects.OfType<IProjectileStandardEffect>().Where(x => x is not IProjectileImpactEffect))
                effect.Execute();

            processedEntities.Add(target.Entity);
        }

        public void LeaveZone(Collider2D collider)
        {
        }
    }
}
