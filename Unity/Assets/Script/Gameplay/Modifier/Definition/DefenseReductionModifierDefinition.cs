using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DefenseReductionModifierDefinition", menuName = "Definition/Modifier/DefenseReductionModifierDefinition")]
    public class DefenseReductionModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, DefenseReductionModifierDefinition>
        {
            private Statistic<float> defenseReduction;

            public Modifier(ModifierHandler modifiable, DefenseReductionModifierDefinition modifierDefinition, float duration, float reduction, IModifierSource source = null) : base(modifiable, modifierDefinition, source)
            {
                With(new CharacterModifierTimeElement(duration));
                defenseReduction = new Statistic<float>(StatisticDefinition.FlatDefense, -reduction);
                StatisticRegistry.Register(defenseReduction);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(defenseReduction);
            }
        }
    }
}
