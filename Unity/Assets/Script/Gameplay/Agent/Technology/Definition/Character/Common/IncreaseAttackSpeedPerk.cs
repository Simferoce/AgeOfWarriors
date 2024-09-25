using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackSpeedPerk", menuName = "Definition/Technology/Common/IncreaseAttackSpeedPerk")]
    public class IncreaseAttackSpeedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackSpeedPerk>
        {
            private StatisticModifiable<float> attackPower = new StatisticModifiable<float>(definition: StatisticRepository.AttackPower);

            public Modifier(ModifierHandler modifiable, IncreaseAttackSpeedPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                attackPower.Initialize(this);
                attackPower.Modify(definition.amount);
            }
        }

        [SerializeField, Range(0, 1)] private float amount;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
