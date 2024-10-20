using Game.Projectile;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class FloatStatisticProjectileParameterFactory : ProjectileParameterFactory
    {
        [SerializeField] private string name;
        [SerializeReference, SubclassSelector] private Statistic statistic;

        public override ProjectileParameter Create(object entity)
        {
            return new StatisticProjectileParameter<float>(name, statistic.Definition, statistic.GetValue<float>(entity));
        }
    }
}
