using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "HealOnShieldEndPerk", menuName = "Definition/Technology/Berserker/HealOnShieldEndPerk")]
    public class HealOnShieldEndPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, HealOnShieldEndPerk>
        {
            public Modifier(ModifierHandler modifiable, HealOnShieldEndPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                modifiable.OnModifierRemoved += Modifiable_ModifierRemoved;
            }

            private void Modifiable_ModifierRemoved(Game.Modifier obj)
            {
                if (obj is not ShieldModifierDefinition.Shield shield)
                    return;

                if (!modifiable.Entity.TryGetCachedComponent<Character>(out Character character))
                    return;

                float heal = definition.healPerShieldPointRemaining * shield.Remaining;
                if (heal <= 0)
                    return;

                character.Heal(heal);
            }

            public override void Dispose()
            {
                base.Dispose();

                modifiable.OnModifierRemoved -= Modifiable_ModifierRemoved;
            }
        }

        [SerializeField] private float healPerShieldPointRemaining;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
