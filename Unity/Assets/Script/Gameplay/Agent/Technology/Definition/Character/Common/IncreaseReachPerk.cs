using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseReachPerk", menuName = "Definition/Technology/Common/IncreaseReachPerk")]
    public class IncreaseReachPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseReachPerk>
        {
            private Statistic<float> reachPercentage;

            public Modifier(ModifierHandler modifiable, IncreaseReachPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                reachPercentage = new Statistic<float>(StatisticDefinition.PercentageReach, definition.amount);
                StatisticRegistry.Register(reachPercentage);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(reachPercentage);
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
