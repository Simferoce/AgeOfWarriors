using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerBaseOnRemainingHealthPerk", menuName = "Definition/Technology/Berserker/IncreaseAttackPowerBaseOnRemainingHealthPerk")]
    public class IncreaseAttackPowerBaseOnRemainingHealthPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackPowerBaseOnRemainingHealthPerk>
        {
            public override float? AttackPower => modifiable.TryGetCachedComponent<Character>(out Character character)
                && character.Health / character.MaxHealth < definition.attackpower
                ? definition.threshold
                : null;

            public Modifier(IModifiable modifiable, IncreaseAttackPowerBaseOnRemainingHealthPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
            }
        }

        [SerializeField] private float attackpower;
        [SerializeField] private float threshold;

        public override string ParseDescription()
        {

            return string.Format(Description, attackpower, threshold);
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
