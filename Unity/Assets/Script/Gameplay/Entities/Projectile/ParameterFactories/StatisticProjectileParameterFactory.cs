using Game.Projectile;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class StatisticProjectileParameterFactory<T> : ProjectileParameterFactory
    {
        [SerializeField] private string name;
        [SerializeField] private StatisticReference<T> reference;

        public override ProjectileParameter Create(object entity)
        {
            return new ProjectileParameter<T>(name, reference.Resolve(entity as Entity).GetValue<T>(null));
        }
    }

    [Serializable]
    public class StatisticProjectileParameterFactoryFloat : StatisticProjectileParameterFactory<float>
    {

    }
}
