using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseReachPerk", menuName = "Definition/Technology/Common/IncreaseReachPerk")]
    public class IncreaseReachPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseReachPerk>
        {
            private Statistic<float> reachPercentage;

            public Modifier(IncreaseReachPerk modifierDefinition) : base(modifierDefinition)
            {
                reachPercentage = new Statistic<float>(StatisticDefinition.PercentageReach, definition.amount);
                StatisticRegistry.Register(reachPercentage);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(reachPercentage);
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
