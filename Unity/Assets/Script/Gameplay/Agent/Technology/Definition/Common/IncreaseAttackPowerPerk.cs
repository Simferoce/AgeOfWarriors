using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerPerk", menuName = "Definition/Technology/Common/IncreaseAttackPowerPerk")]
    public class IncreaseAttackPowerPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackPowerPerk>
        {
            public override float? AttackPower => definition.amount;

            public Modifier(IModifiable modifiable, IncreaseAttackPowerPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
            }
        }

        [SerializeField] private float amount;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifiable.GetCachedComponent<IModifierSource>());
        }

        public override string ParseDescription()
        {
            return string.Format(Description, amount);
        }
    }
}
