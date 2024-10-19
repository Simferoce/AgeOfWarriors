using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class ParameterReferenceModifierStatistic : ModifierStatistic
    {
        [SerializeField] private string name;

        public override T GetValue<T>(object context)
        {
            if (TryResolve(context, out StatisticModifierParameter<T> modifierStatistic))
                return modifierStatistic.Value;

            return default(T);
        }

        private bool TryResolve<T>(object context, out T statistic)
            where T : StatisticModifierParameter
        {
            statistic = null;

            if (context is not ModifierEntity modifier)
            {
                Debug.LogError($"Expecting the object type of {context} to be of {nameof(ModifierEntity)}");
                return false;
            }

            ModifierParameter modifierParameter = modifier.Parameters.FirstOrDefault(x => x.Name == name);
            if (modifierParameter == null)
            {
                Debug.LogError($"Expecting a parameter of name: {name}", modifier);
                return false;
            }

            if (modifierParameter is not T modifierParameterStatistic)
            {
                Debug.LogError($"Expecting the parameter {name} to be of type {nameof(T)}");
                return false;
            }

            statistic = modifierParameterStatistic;
            return true;
        }
    }
}
