using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseSpeedPowerPerk", menuName = "Definition/Technology/Common/IncreaseSpeedPowerPerk")]
    public class IncreaseSpeedPowerPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseSpeedPowerPerk>
        {
            public override float? SpeedPercentage => definition.amount;

            public Modifier(ModifierHandler modifiable, IncreaseSpeedPowerPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
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
