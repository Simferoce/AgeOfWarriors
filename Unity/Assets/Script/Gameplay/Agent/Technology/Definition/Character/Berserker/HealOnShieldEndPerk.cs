using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "HealOnShieldEndPerk", menuName = "Definition/Technology/Berserker/HealOnShieldEndPerk")]
    public class HealOnShieldEndPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, HealOnShieldEndPerk>
        {
            public Modifier(HealOnShieldEndPerk modifierDefinition) : base(modifierDefinition)
            {
            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
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

        public override string ParseDescription()
        {
            return string.Format(Description, healPerShieldPointRemaining);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
