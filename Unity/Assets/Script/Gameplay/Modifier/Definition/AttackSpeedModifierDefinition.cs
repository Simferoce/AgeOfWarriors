using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackSpeedModifierDefinition", menuName = "Definition/Modifier/AttackSpeedModifierDefinition")]
    public class AttackSpeedModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, AttackSpeedModifierDefinition>
        {
            private Statistic<float> attackSpeedPercentage;

            public Modifier(ModifierHandler modifiable, AttackSpeedModifierDefinition modifierDefinition, float amount, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                attackSpeedPercentage = new Statistic<float>(StatisticDefinition.PercentageAttackSpeed, amount);
                StatisticRegistry.Register(attackSpeedPercentage);
            }

            public override string ParseDescription()
            {
                return string.Format(definition.Description, attackSpeedPercentage.GetValue());
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(attackSpeedPercentage);
            }
        }
    }
}
