using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ModifierStatisticParameterReference : ModifierStatistic
    {
        [SerializeField] private string name;

        public override StatisticDefinition GetDefinition(object context)
        {
            if (TryResolve(context, out ModifierParameterStatistic modifierStatistic))
                return modifierStatistic.StatisticDefinition;

            return null;
        }

        public override T GetValue<T>(object context)
        {
            if (TryResolve(context, out ModifierParameterStatistic<T> modifierStatistic))
                return modifierStatistic.Value;

            return default(T);
        }

        private bool TryResolve<T>(object context, out T statistic)
            where T : ModifierParameterStatistic
        {
            statistic = null;

            if (context is not Modifier modifier)
            {
                Debug.LogError($"Expecting the object type of {context} to be of {nameof(Modifier)}");
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
