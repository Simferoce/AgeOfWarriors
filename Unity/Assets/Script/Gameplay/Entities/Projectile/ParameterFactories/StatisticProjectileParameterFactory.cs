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
        [SerializeReference, SubclassSelector] private Value value;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            value.Initialize(entity);
        }

        public override ProjectileParameter Create(object entity)
        {
            return new ProjectileParameter<T>(name, value.GetValue<T>());
        }
    }

    [Serializable]
    public class StatisticProjectileParameterFactoryFloat : StatisticProjectileParameterFactory<float>
    {

    }
}
