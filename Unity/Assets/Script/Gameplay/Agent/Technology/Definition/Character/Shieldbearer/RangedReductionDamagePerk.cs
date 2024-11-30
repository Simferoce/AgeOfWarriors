using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "RangedReductionDamagePerk", menuName = "Definition/Technology/Shieldbearer/RangedReductionDamagePerk")]
    public class RangedReductionDamagePerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, RangedReductionDamagePerk>
        {
            private Statistic<float> rangedDamageReduction;

            public Modifier(RangedReductionDamagePerk modifierDefinition) : base(modifierDefinition)
            {
                rangedDamageReduction = new Statistic<float>(StatisticDefinition.RangedPercentageDamageTaken, definition.rangedDamageReduction);
                StatisticRegistry.Register(rangedDamageReduction);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(rangedDamageReduction);
            }
        }

        [SerializeField, Range(0, 1)] private float rangedDamageReduction;

        public override string ParseDescription()
        {
            return string.Format(Description, rangedDamageReduction);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
