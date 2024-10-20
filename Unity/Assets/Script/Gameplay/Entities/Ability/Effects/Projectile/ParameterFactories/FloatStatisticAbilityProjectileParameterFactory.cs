using Game.Projectile;
using Game.Statistics;
using System;
using UnityEngine;

namespace Game.Ability
{
    [Serializable]
    public class FloatStatisticAbilityProjectileParameterFactory : AbilityProjectileParameterFactory
    {
        [SerializeField] private string name;
        [SerializeReference, SubclassSelector] private Statistic statistic;

        public override ProjectileParameter Create(AbilityEntity ability)
        {
            return new StatisticProjectileParameter<float>(name, statistic.Definition, statistic.GetValue<float>(ability));
        }
    }
}
