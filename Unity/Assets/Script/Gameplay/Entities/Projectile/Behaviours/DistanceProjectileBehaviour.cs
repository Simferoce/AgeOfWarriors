using Game.Statistics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class DistanceProjectileBehaviour : ProjectileBehaviour
    {
        [SerializeField] private StatisticReference<float> range;
        [SerializeReference, SubclassSelector] private List<IProjectileStandardEffect> effects;

        private Vector3 previousPosition;
        private float distanceTraveled;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);
            previousPosition = projectile.transform.position;

            range.Initialize(projectile);

            foreach (IProjectileStandardEffect effect in effects)
                effect.Initialize(projectile);
        }

        public override void Update()
        {
            base.Update();
            Vector3 currentPosition = projectile.transform.position;
            float delta = Vector3.Distance(previousPosition, currentPosition);
            distanceTraveled += delta;

            previousPosition = currentPosition;
            if (distanceTraveled < range.GetOrThrow().Get<float>())
                return;

            foreach (IProjectileStandardEffect effect in effects)
                effect.Execute();
        }
    }
}
