using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseSpeedPowerPerk", menuName = "Definition/Technology/Common/IncreaseSpeedPowerPerk")]
    public class IncreaseSpeedPowerPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseSpeedPowerPerk>
        {
            private StatisticModifiable<float> speedPercentage = new StatisticModifiable<float>(definition: StatisticRepository.SpeedPercentage);

            public Modifier(ModifierHandler modifiable, IncreaseSpeedPowerPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                speedPercentage.Initialize(this);
                speedPercentage.Modify(definition.amount);
            }
        }

        [SerializeField, Range(0, 1)] private float amount;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
