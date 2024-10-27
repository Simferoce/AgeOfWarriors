using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class ModifierStackStatisticValue : StatisticValue<float>
    {
        [SerializeReference, SubclassSelector] private StatisticValue multiplier;

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            multiplier.Initialize(owner);
        }

        public override string GetDescription(Context context)
        {
            return $"({multiplier.GetDescription(context)} * stack)";
        }

        public override float GetValue(Context context)
        {
            if (owner is not ModifierEntity modifier)
            {
                Debug.LogError($"Expecting the owner of {nameof(ModifierStackStatisticValue)} to be {nameof(ModifierEntity)} but instead got {owner.GetType().Name}", owner);
                return default;
            }

            IModifierStack modifierBehaviour = modifier.Behaviours.FirstOrDefault(x => x is IModifierStack) as IModifierStack;
            if (modifierBehaviour == null)
            {
                Debug.LogError($"Expecting the modifier to own a {nameof(IModifierStack)}.", modifier);
                return default;
            }

            return multiplier.GetValue<float>(context) * modifierBehaviour.CurrentStack;
        }
    }
}
