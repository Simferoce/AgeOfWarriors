using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackSpeedPerk", menuName = "Definition/Technology/Common/IncreaseAttackSpeedPerk")]
    public class IncreaseAttackSpeedPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackSpeedPerk>
        {
            private Statistic<float> attackSpeed;

            public Modifier(IncreaseAttackSpeedPerk modifierDefinition) : base(modifierDefinition)
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
