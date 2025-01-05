using Game.Modifier;
using System.Linq;
using UnityEngine;

namespace Game.UI.Windows
{
    public class ModifierInstanceInspectable : IModifierInspectable
    {
        private ModifierEntity modifier;

        public ModifierInstanceInspectable(ModifierEntity modifier)
        {
            this.modifier = modifier;
        }

        public float? GetStack()
        {
            return modifier?.Behaviours.OfType<IModifierStack>().FirstOrDefault()?.CurrentStack;
        }

        public float? GetPercentageRemainingDuration()
        {
            return modifier?.Behaviours.OfType<IModifierDuration>().FirstOrDefault()?.GetPercentageRemainingDuration();
        }

        public string GetTitle()
        {
            return modifier.GetDefinition().Title;
        }

        public Sprite GetIcon()
        {
            return modifier.GetDefinition().Icon;
        }

        public string GetDescription()
        {
            return modifier.GetDefinition().ParseDescription(null);
        }
    }
}
