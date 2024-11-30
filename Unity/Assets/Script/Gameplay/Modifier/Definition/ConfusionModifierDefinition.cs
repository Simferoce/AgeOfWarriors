using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ConfusionModifierDefinition", menuName = "Definition/Modifier/ConfusionModifierDefinition")]
    public class ConfusionModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ConfusionModifierDefinition>
        {
            public override bool? IsConfused => true;

            public Modifier(ConfusionModifierDefinition modifierDefinition) : base(modifierDefinition)
            {
            }
        }

        public override Game.Modifier Instantiate()
        {
            throw new System.NotImplementedException();
        }

    }
}
