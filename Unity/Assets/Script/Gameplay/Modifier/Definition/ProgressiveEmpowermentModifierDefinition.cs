using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ProgressiveEmpowermentModifierDefinition", menuName = "Definition/Modifier/ProgressiveEmpowermentModifierDefinition")]
    public class ProgressiveEmpowermentModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ProgressiveEmpowermentModifierDefinition>
        {
            private StackModifierElement stackModifierElement;

            public Modifier(IModifiable modifiable, ProgressiveEmpowermentModifierDefinition modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                stackModifierElement = new StackModifierElement();
                this.With(stackModifierElement);

                stackModifierElement.OnStackGained += StackModifierElement_OnStackGained;
                stackModifierElement.IncreaseStack();
            }

            private void StackModifierElement_OnStackGained(StackModifierElement stackModifierElement)
            {
                if (stackModifierElement.CurrentStack > definition.maxStack)
                {
                    stackModifierElement.OnStackGained -= StackModifierElement_OnStackGained;

                    EmpoweredModifierDefinition empoweredModifierDefinition = (Definition as ProgressiveEmpowermentModifierDefinition).empoweredModifierDefinition;
                    Game.Modifier modifier = modifiable.GetModifiers().FirstOrDefault(x => x.Definition == empoweredModifierDefinition);
                    if (modifier == null)
                    {
                        modifiable.AddModifier(new EmpoweredModifierDefinition.Modifier(
                            modifiable,
                            empoweredModifierDefinition,
                            Source));
                    }
                    else
                    {
                        modifier.Refresh();
                    }

                    modifiable.RemoveModifier(this);
                }
            }
        }

        [SerializeField] private int maxStack;
        [SerializeField] private EmpoweredModifierDefinition empoweredModifierDefinition;

        public int MaxStack => maxStack;
        public EmpoweredModifierDefinition EmpoweredModifierDefinition => empoweredModifierDefinition;
    }
}