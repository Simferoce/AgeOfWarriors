using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackSpeedPerk", menuName = "Definition/Technology/Common/IncreaseAttackSpeedPerk")]
    public class IncreaseAttackSpeedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackSpeedPerk>
        {
            public override float? AttackPower => definition.amount;

            public Modifier(IModifiable modifiable, IncreaseAttackSpeedPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
            }
        }

        [SerializeField, Range(0, 1)] private float amount;

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
