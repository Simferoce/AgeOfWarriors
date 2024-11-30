using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseDefensePerk", menuName = "Definition/Technology/Common/IncreaseDefensePerk")]
    public class IncreaseDefensePerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseDefensePerk>
        {
            private Statistic<float> defenseFlat;

            public Modifier(IncreaseDefensePerk modifierDefinition) : base(modifierDefinition)
            {
                defenseFlat = new Statistic<float>(StatisticDefinition.FlatDefense, definition.amount);
                StatisticRegistry.Register(defenseFlat);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(defenseFlat);
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
