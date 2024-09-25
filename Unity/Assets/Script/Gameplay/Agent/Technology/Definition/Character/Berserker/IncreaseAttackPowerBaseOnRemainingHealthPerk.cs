using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerBaseOnRemainingHealthPerk", menuName = "Definition/Technology/Berserker/IncreaseAttackPowerBaseOnRemainingHealthPerk")]
    public class IncreaseAttackPowerBaseOnRemainingHealthPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackPowerBaseOnRemainingHealthPerk>
        {
            //public override float? AttackPower => modifiable.Entity.TryGetCachedComponent<Character>(out Character character)
            //    && character.Health / character.MaxHealth < definition.attackpower
            //    ? definition.threshold
            //    : null;

            public Modifier(ModifierHandler modifiable, IncreaseAttackPowerBaseOnRemainingHealthPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
            }
        }

        [SerializeField] private float attackpower;
        [SerializeField] private float threshold;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
