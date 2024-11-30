using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerPerk", menuName = "Definition/Technology/Common/IncreaseAttackPowerPerk")]
    public class IncreaseAttackPowerPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackPowerPerk>
        {
            private Statistic<float> attackPowerFlat;

            public Modifier(IncreaseAttackPowerPerk modifierDefinition) : base(modifierDefinition)
            {
                attackPowerFlat = new Statistic<float>(StatisticDefinition.FlatAttackPower, definition.amount);
                StatisticRegistry.Register(attackPowerFlat);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(attackPowerFlat);
            }
        }

        [SerializeField] private float amount;

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
