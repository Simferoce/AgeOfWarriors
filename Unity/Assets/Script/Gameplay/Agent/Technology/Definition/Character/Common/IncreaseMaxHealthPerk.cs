using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseMaxHealthPerk", menuName = "Definition/Technology/Common/IncreaseMaxHealthPerk")]
    public class IncreaseMaxHealthPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseMaxHealthPerk>
        {
            private Statistic<float> maxHealthFlat;

            public Modifier(IncreaseMaxHealthPerk modifierDefinition) : base(modifierDefinition)
            {
                maxHealthFlat = new Statistic<float>(StatisticDefinition.FlatMaxHealth, definition.amount);
                StatisticRegistry.Register(maxHealthFlat);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(maxHealthFlat);
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
