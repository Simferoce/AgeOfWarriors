using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "EmpoweredModifierDefinition", menuName = "Definition/Modifier/EmpoweredModifierDefinition")]
    public class EmpoweredModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private StackModifierElement stackModifierElement;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition) : base(modifiable, modifierDefinition)
            {
                stackModifierElement = new StackModifierElement();
                stackModifierElement.IncreaseStack();

                With(stackModifierElement);
            }

            public void Consume()
            {
                if (stackModifierElement.CurrentStack > 1)
                    stackModifierElement.DecreaseStack();
                else
                    modifiable.RemoveModifier(this);
            }
        }
    }
}
