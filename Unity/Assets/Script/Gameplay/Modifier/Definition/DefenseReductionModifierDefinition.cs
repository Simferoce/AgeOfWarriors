using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DefenseReductionModifierDefinition", menuName = "Definition/Modifier/DefenseReductionModifierDefinition")]
    public class DefenseReductionModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, DefenseReductionModifierDefinition>
        {
            public override float? DefenseReduction => defenseReduction;

            private float defenseReduction;

            public Modifier(IModifiable modifiable, DefenseReductionModifierDefinition modifierDefinition, float duration, float reduction, IModifierSource source = null) : base(modifiable, modifierDefinition, source)
            {
                With(new CharacterModifierTimeElement(duration));
                defenseReduction = reduction;
            }
        }
    }
}
