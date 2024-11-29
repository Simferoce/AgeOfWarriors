using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackSpeedPerk", menuName = "Definition/Technology/Common/IncreaseAttackSpeedPerk")]
    public class IncreaseAttackSpeedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackSpeedPerk>
        {
            private Statistic<float> attackSpeed;

            public Modifier(ModifierHandler modifiable, IncreaseAttackSpeedPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                attackSpeed = new Statistic<float>(StatisticDefinition.PercentageAttackSpeed, definition.amount);
                StatisticRegistry.Register(attackSpeed);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(attackSpeed);
            }
        }

        [SerializeField, Range(0, 1)] private float amount;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }

        public override string ParseDescription()
        {
            return string.Format(Description, amount);
        }
    }
}
