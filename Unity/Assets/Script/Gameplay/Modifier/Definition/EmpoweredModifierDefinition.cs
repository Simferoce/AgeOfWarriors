using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "EmpoweredModifierDefinition", menuName = "Definition/Modifier/EmpoweredModifierDefinition")]
    public class EmpoweredModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, EmpoweredModifierDefinition>
        {
            private StackModifierElement stackModifierElement;
            public float PercentageDamageIncrease => definition.percentageDamageIncrease;

            public Modifier(ModifierHandler modifiable, EmpoweredModifierDefinition modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
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

        [SerializeField, Range(0, 5)] private float percentageDamageIncrease;

        public float PercentageDamageIncrease => percentageDamageIncrease;
    }
}
