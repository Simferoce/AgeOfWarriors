using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class PerStackMultiplierModifierStatistic : ModifierStatistic
    {
        [SerializeReference, SubclassSelector] private Statistic multiplier;

        public override T GetValue<T>(object context)
        {
            if (context is not ModifierEntity modifier)
            {
                Debug.LogError($"Expecting the {context} to be of type {nameof(ModifierEntity)}.");
                return default;
            }

            StackModifierBehaviour stackModifierBehaviour = modifier.Behaviours.FirstOrDefault(x => x is StackModifierBehaviour) as StackModifierBehaviour;
            if (stackModifierBehaviour == null)
            {
                Debug.LogError($"Expecting the {modifier} to own {nameof(StackModifierBehaviour)}.", modifier);
                return default;
            }

            return StatisticConverter.ConvertGeneric<T, float>(Mathf.Pow(multiplier.GetValue<float>(context), stackModifierBehaviour.CurrentStack));
        }
    }
}
