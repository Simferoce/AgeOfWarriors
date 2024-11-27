using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DamageDealtReductionModifierDefinition", menuName = "Definition/Modifier/DamageDealtReductionModifierDefinition")]
    public class DamageDealtReductionModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, DamageDealtReductionModifierDefinition>
        {
            public class Instancier : Instancier<DamageDealtReductionModifierDefinition>
            {
                [SerializeField] private float amount;
                [SerializeField] private float duration;

                public float Amount { get => amount; set => amount = value; }
                public float Duration { get => duration; set => duration = value; }

                public override Game.Modifier Instantiate(ModifierHandler modifiable, IModifierSource source)
                {
                    return new Modifier(modifiable, definition, amount, source).With(new CharacterModifierTimeElement(duration));
                }
            }

            public override float? DamageDealtReduction => amount;

            private float amount;

            public Modifier(ModifierHandler modifiable, DamageDealtReductionModifierDefinition modifierDefinition, float amount, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                this.amount = amount;
            }

            public override string ParseDescription()
            {
                return string.Format(definition.Description, amount);
            }
        }
    }
}
