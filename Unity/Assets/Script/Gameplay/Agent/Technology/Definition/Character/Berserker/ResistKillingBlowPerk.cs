using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ResistKillingBlowPerk", menuName = "Definition/Technology/Berserker/ResistKillingBlowPerk")]
    public class ResistKillingBlowPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, ResistKillingBlowPerk>
        {
            private bool hasResists = false;

            public Modifier(ModifierHandler modifiable, ResistKillingBlowPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
            }

            public bool CanResistsKillingBlow()
            {
                return !hasResists;
            }

            public void ResistKillingBlow()
            {
                if (!hasResists)
                    modifiable.AddModifier(
                        new ResistDeathModifierDefinition.ResistDeath(
                            modifiable,
                            definition.resistDeathModifierDefinition,
                            Source)
                        .With(new CharacterModifierTimeElement(definition.duration)));
            }
        }

        [SerializeField] private float duration;
        [SerializeField] private ResistDeathModifierDefinition resistDeathModifierDefinition;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
