using Game.Ability;
using Game.Statistics;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Modifier
{
    [Serializable]
    public class ModifierAbilityStatisticValue<T> : StatisticValue<T>
    {
        [SerializeReference, SubclassSelector] private StatisticValue<T> value;

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            Assert.IsTrue(owner is ModifierEntity, $"Expecting the owner to be of type {nameof(ModifierEntity)} but instead it is {owner.Parent.GetType().Name}.");
            Assert.IsTrue((owner as ModifierEntity).Applier.Entity, $"Expecting the source of the modifier to be of type {nameof(AbilityEntity)} but instead it is {owner.Parent.GetType().Name}.");
            value.Initialize((owner as ModifierEntity).Applier.Entity);
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
    public class ModifierAbilityStatisticValue : ModifierAbilityStatisticValue<float> { }
}
