using Game.Projectile;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class StatisticProjectileParameterFactory<T> : ProjectileParameterFactory
    {
        [SerializeReference, SubclassSelector] private Statistic statistic;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            statistic.Initialize(entity);
        }

        public override ProjectileParameter Create(object entity)
        {
            return new ProjectileParameter<T>(statistic.Name, statistic.GetValue<T>(null));
        }
    }

    [Serializable]
    public class StatisticProjectileParameterFactoryFloat : StatisticProjectileParameterFactory<float>
    {

    }
}
