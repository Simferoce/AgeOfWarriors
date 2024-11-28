using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseAttackPowerPerk", menuName = "Definition/Technology/Common/IncreaseAttackPowerPerk")]
    public class IncreaseAttackPowerPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseAttackPowerPerk>
        {
            private Statistic<float> attackPowerFlat;

            public Modifier(ModifierHandler modifiable, IncreaseAttackPowerPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                attackPowerFlat = new Statistic<float>(StatisticDefinition.AttackPowerFlat, definition.amount);
                StatisticRegistry.Register(attackPowerFlat);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(attackPowerFlat);
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
