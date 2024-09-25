using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "IncreaseMaxHealthPerk", menuName = "Definition/Technology/Common/IncreaseMaxHealthPerk")]
    public class IncreaseMaxHealthPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, IncreaseMaxHealthPerk>
        {
            private StatisticModifiable<float> maxHealth = new StatisticModifiable<float>(definition: StatisticRepository.MaxHealth);

            public Modifier(ModifierHandler modifiable, IncreaseMaxHealthPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                maxHealth.Initialize(this);
                maxHealth.Modify(definition.amount);
            }
        }

        [SerializeField] private float amount;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
