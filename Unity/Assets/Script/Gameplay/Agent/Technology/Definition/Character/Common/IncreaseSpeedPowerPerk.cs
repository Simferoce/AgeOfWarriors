using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseSpeedPowerPerk", menuName = "Definition/Technology/Common/IncreaseSpeedPowerPerk")]
    public class IncreaseSpeedPowerPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseSpeedPowerPerk>
        {
            private Statistic<float> speed;

            public Modifier(ModifierHandler modifiable, IncreaseSpeedPowerPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                speed = new Statistic<float>(StatisticDefinition.SpeedPercentage, definition.amount);
                StatisticRegistry.Register(speed);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(speed);
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
