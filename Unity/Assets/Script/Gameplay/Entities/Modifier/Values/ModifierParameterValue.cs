using Game.Extensions;
using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class ModifierParameterValue<T> : Value<T>
    {
        [SerializeField] private string name;

        public override T GetValue()
        {
            if (owner is not ModifierEntity modifier)
            {
                Debug.LogError($"Expecting the owner to be of type {nameof(ModifierEntity)} but instead got {owner.GetType()}.");
                return default;
            }

            ModifierParameter<T> modifierParameter = modifier.Parameters.OfType<ModifierParameter<T>>().FirstOrDefault(x => x.Name == name);
            if (modifierParameter == null)
            {
                Debug.LogError($"Did not find a parameter with name \"{name}\" with type {nameof(T)} in \"{modifier.transform.GetFullPath()}\"", modifier);
                return default;
            }

            return modifierParameter.GetValue();
        }

        public override bool TryGetDescription(out string description)
        {
            description = string.Empty;
            return false;
        }
    }

    [Serializable]
    public class ModifierParameterValueFloat : ModifierParameterValue<float>
    {

    }
}
