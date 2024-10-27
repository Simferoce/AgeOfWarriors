using Game.Ability;
using Game.Statistics;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Projectile
{
    [Serializable]
    public class ProjectileAbilityStatisticValue<T> : StatisticValue<T>
    {
        [SerializeReference, SubclassSelector] private StatisticValue<T> value;

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            Assert.IsTrue(owner.Parent is AbilityEntity, $"Expecting the parent of {owner} to be of type {nameof(AbilityEntity)} but instead it is {owner.Parent.GetType().Name}.");
            value.Initialize(owner.Parent);
        }

        public override string GetDescription(Context context)
        {
            return value.GetDescription(context);
        }

        public override T GetValue(Context context)
        {
            return value.GetValue(context);
        }
    }

    [Serializable]
    public class ProjectileAbilityStatisticValueFloat : ProjectileAbilityStatisticValue<float> { }
}
