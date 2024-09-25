using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerPerk", menuName = "Definition/Technology/Common/IncreaseAttackPowerPerk")]
    public class IncreaseAttackPowerPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackPowerPerk>
        {
            private StatisticModifiable<float> attackPower = new StatisticModifiable<float>(definition: StatisticRepository.AttackPower);

            public Modifier(ModifierHandler modifiable, IncreaseAttackPowerPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                attackPower.Initialize(this);
                attackPower.Modify(definition.amount);
            }
        }

        [SerializeField] private float amount;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
