using Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class OnHitProjectileBehaviour : ProjectileBehaviour
    {
        [SerializeReference, SubclassSelector] private List<ProjectileTargetFilter> filters;
        [SerializeReference, SubclassSelector] private OnHitProjectileBehaviourTargetProvider target;
        [SerializeReference, SubclassSelector] private List<ProjectileEffect> effects;

        private AttackFactory attackFactory;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);
            target.Initialize(projectile);
            foreach (ProjectileTargetFilter filter in filters)
                filter.Initialize(projectile);

            foreach (ProjectileEffect effect in effects)
                effect.Initialize(projectile);

            attackFactory = projectile.AddOrGetCachedComponent<AttackFactory>();
            attackFactory.OnAttackLanded += OnAttackLanded;
        }

        private void OnAttackLanded(AttackResult attackResult)
        {
            Entity entity = target.Execute(attackResult);
            if (entity == null)
            {
                Debug.LogError($"Expecting {target.GetType().Name} to provide a non null value.", projectile);
                return;
            }

            foreach (IProjectileImpactEffect effect in effects.Cast<IProjectileImpactEffect>())
                effect.Execute(entity);

            foreach (IProjectileStandardEffect effect in effects.Where(x => x is not IProjectileImpactEffect).Cast<IProjectileStandardEffect>())
                effect.Execute();
        }

        public override void Dispose()
        {
            base.Dispose();
            attackFactory.OnAttackLanded -= OnAttackLanded;
        }

        public override bool Validate(ProjectileEntity projectile)
        {
            bool changed = base.Validate(projectile);

            if (target != null)
                changed |= target.Validate(projectile);

            foreach (ProjectileTargetFilter filter in filters.Where(x => x != null))
                filter.Validate(projectile);

            foreach (ProjectileEffect effect in effects.Where(x => x != null))
                effect.Validate(projectile);

            return changed;
        }
    }
}
