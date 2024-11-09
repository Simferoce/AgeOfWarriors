using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class ModifierStackValue : Value<float>
    {
        [SerializeReference, SubclassSelector] private Value multiplier;

        public override void Initialize(Entity owner)
        {
            base.Initialize(owner);
            multiplier.Initialize(owner);
        }

        public override bool TryGetDescription(out string description)
        {
            description = $"...";
            return true;
        }

        public override float GetValue()
        {
            if (owner is not ModifierEntity modifier)
            {
                Debug.LogError($"Expecting the owner of {nameof(ModifierStackValue)} to be {nameof(ModifierEntity)} but instead got {owner.GetType().Name}", owner);
                return default;
            }

            IModifierStack modifierBehaviour = modifier.Behaviours.FirstOrDefault(x => x is IModifierStack) as IModifierStack;
            if (modifierBehaviour == null)
            {
                Debug.LogError($"Expecting the modifier to own a {nameof(IModifierStack)}.", modifier);
                return default;
            }

            return multiplier.GetValue<float>() * modifierBehaviour.CurrentStack;
        }
    }
}
