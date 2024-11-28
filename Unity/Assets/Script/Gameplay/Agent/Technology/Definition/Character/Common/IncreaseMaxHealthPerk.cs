using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseMaxHealthPerk", menuName = "Definition/Technology/Common/IncreaseMaxHealthPerk")]
    public class IncreaseMaxHealthPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseMaxHealthPerk>
        {
            private Statistic<float> maxHealthFlat;

            public Modifier(ModifierHandler modifiable, IncreaseMaxHealthPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                maxHealthFlat = new Statistic<float>(StatisticDefinition.MaxHealthFlat, definition.amount);
                StatisticRegistry.Register(maxHealthFlat);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(maxHealthFlat);
            }
        }

        [SerializeField] private float amount;

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
