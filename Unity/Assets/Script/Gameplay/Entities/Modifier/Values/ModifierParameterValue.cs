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
            if (owner.Owner is not ModifierEntity modifier)
            {
                Debug.LogError($"Expecting the owner to be of type {nameof(ModifierEntity)} but instead got {owner.GetType()}.");
                return default;
            }

            ModifierParameter modifierParameter = modifier.Parameters.OfType<ModifierParameter>().FirstOrDefault(x => x.Name == name);
            if (modifierParameter == null)
            {
                Debug.LogError($"Did not find a parameter with name \"{name}\" in \"{modifier.transform.GetFullPath()}\"", modifier);
                return default;
            }

            return modifierParameter.GetValue<T>();
        }
    }

    [Serializable]
    public class ModifierParameterValueFloat : ModifierParameterValue<float>
    {

    }

    [Serializable]
    public class ModifierParameterValueInteger : ModifierParameterValue<int>
    {

    }

    [Serializable]
    public class ModifierParameterValueEntity : ModifierParameterValue<Entity>
    {

    }
}
