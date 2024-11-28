using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "RangedReductionDamagePerk", menuName = "Definition/Technology/Shieldbearer/RangedReductionDamagePerk")]
    public class RangedReductionDamagePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, RangedReductionDamagePerk>
        {
            private Statistic<float> rangedDamageReduction;

            public Modifier(ModifierHandler modifiable, RangedReductionDamagePerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                rangedDamageReduction = new Statistic<float>(StatisticDefinition.RangedDamageTaken, definition.rangedDamageReduction);
                StatisticRegistry.Register(rangedDamageReduction);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(rangedDamageReduction);
            }
        }

        [SerializeField, Range(0, 1)] private float rangedDamageReduction;

        public override string ParseDescription()
        {
            return string.Format(Description, rangedDamageReduction);
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
