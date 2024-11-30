using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseSpeedPowerPerk", menuName = "Definition/Technology/Common/IncreaseSpeedPowerPerk")]
    public class IncreaseSpeedPowerPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseSpeedPowerPerk>
        {
            private Statistic<float> speed;

            public Modifier(IncreaseSpeedPowerPerk modifierDefinition) : base(modifierDefinition)
            {
                speed = new Statistic<float>(StatisticDefinition.PercentageSpeed, definition.amount);
                StatisticRegistry.Register(speed);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(speed);
            }
        }

        [SerializeField, Range(0, 1)] private float amount;

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }

        public override string ParseDescription()
        {
            return string.Format(Description, amount);
        }
    }
}
