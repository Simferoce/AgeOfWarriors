using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseDefensePerk", menuName = "Definition/Technology/Common/IncreaseDefensePerk")]
    public class IncreaseDefensePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseDefensePerk>
        {
            private Statistic<float> defenseFlat;

            public Modifier(ModifierHandler modifiable, IncreaseDefensePerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                defenseFlat = new Statistic<float>(StatisticDefinition.DefenseFlat, definition.amount);
                StatisticRegistry.Register(defenseFlat);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(defenseFlat);
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
