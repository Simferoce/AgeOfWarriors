using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class ModifierStackValue : Value<float>
    {
        public override float GetValue()
        {
            if (owner.Owner is not ModifierEntity modifier)
            {
                Debug.LogError($"Expecting the owner of {nameof(ModifierStackValue)} to be {nameof(ModifierEntity)} but instead got {owner.GetType().Name}");
                return default;
            }

            IModifierStack modifierBehaviour = modifier.Behaviours.FirstOrDefault(x => x is IModifierStack) as IModifierStack;
            if (modifierBehaviour == null)
            {
                Debug.LogError($"Expecting the modifier to own a {nameof(IModifierStack)}.", modifier);
                return default;
            }

            return modifierBehaviour.CurrentStack;
        }
    }
}
