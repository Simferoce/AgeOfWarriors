using Game.Statistics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class TimeProjectileBehaviour : ProjectileBehaviour
    {
        [SerializeField] private StatisticReference<float> duration;
        [SerializeReference, SubclassSelector] private List<IProjectileStandardEffect> effects;

        private float startedAt;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);
            startedAt = Time.time;

            duration.Initialize(projectile);

            foreach (IProjectileStandardEffect effect in effects)
                effect.Initialize(projectile);
        }

        public override void Update()
        {
            base.Update();

            if (startedAt + duration.GetOrThrow().GetModifiedValue<float>(Context.Empty) > Time.time)
                return;

            foreach (IProjectileStandardEffect effect in effects)
                effect.Execute();
        }
    }
}
