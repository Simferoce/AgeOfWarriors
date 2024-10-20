using Game.Statistics;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class TimeProjectileBehaviour : ProjectileBehaviour
    {
        [SerializeReference, SubclassSelector] private Statistic duration;
        [SerializeReference, SubclassSelector] private List<IProjectileStandardEffect> effects;

        private float startedAt;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);
            startedAt = Time.time;

            foreach (IProjectileStandardEffect effect in effects)
                effect.Initialize(projectile);
        }

        public override bool Validate(ProjectileEntity projectile)
        {
            bool changed = base.Validate(projectile);
            if (duration != null && duration.Definition != StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Duration))
            {
                duration.Definition = StatisticDefinitionRepository.Instance.GetById(StatisticIdentifiant.Duration);
                changed = true;
            }

            return changed;
        }

        public override void Update()
        {
            base.Update();

            if (startedAt + duration.GetValue<float>(projectile) > Time.time)
                return;

            foreach (IProjectileStandardEffect effect in effects)
                effect.Execute();
        }
    }
}
