

using Game.Statistics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class DistanceProjectileBehaviour : ProjectileBehaviour
    {
        [SerializeReference, SubclassSelector] private Statistic range;
        [SerializeReference, SubclassSelector] private List<IProjectileStandardEffect> effects;

        private Vector3 previousPosition;
        private float distanceTraveled;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);
            previousPosition = projectile.transform.position;

            foreach (IProjectileStandardEffect effect in effects)
                effect.Initialize(projectile);
        }

        public override bool Validate(ProjectileEntity projectile)
        {
            bool changed = base.Validate(projectile);
            if (range != null && range.Definition != StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Range))
            {
                range.Definition = StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Range);
                changed |= true;
            }

            foreach (IProjectileStandardEffect effect in effects)
                changed |= effect.Validate(projectile);

            return changed;
        }

        public override void Update()
        {
            base.Update();
            Vector3 currentPosition = projectile.transform.position;
            float delta = Vector3.Distance(previousPosition, currentPosition);
            distanceTraveled += delta;

            previousPosition = currentPosition;
            if (distanceTraveled < range.GetValue<float>(projectile))
                return;

            foreach (IProjectileStandardEffect effect in effects)
                effect.Execute();
        }
    }
}
