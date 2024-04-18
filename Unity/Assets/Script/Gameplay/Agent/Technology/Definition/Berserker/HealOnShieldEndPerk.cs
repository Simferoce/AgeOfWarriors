using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "HealOnShieldEndPerk", menuName = "Definition/Technology/Berserker/HealOnShieldEndPerk")]
    public class HealOnShieldEndPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition) : base(modifiable, modifierDefinition)
            {
                if (modifiable.TryGetCachedComponent<IShieldable>(out IShieldable shieldable))
                {
                    shieldable.OnShieldableDestroyed += Shieldable_OnDestroyed;
                }

                modifiable.ModifierRemoved += Modifiable_ModifierRemoved;
            }

            private void Modifiable_ModifierRemoved(Game.Modifier obj)
            {
                if (obj is not ShieldModifierDefinition.Shield shield)
                    return;

                if (!modifiable.TryGetCachedComponent<Character>(out Character character))
                    return;

                float heal = Definition.GetValueOrThrow<float>(this, StatisticDefinition.Heal) * shield.Remaining;
                if (heal <= 0)
                    return;

                character.Heal(heal);
            }

            private void Shieldable_OnDestroyed(IShieldable shieldable)
            {
                shieldable.OnShieldableDestroyed -= Shieldable_OnDestroyed;
                modifiable.ModifierRemoved -= Modifiable_ModifierRemoved;
            }

            public override void Dispose()
            {
                base.Dispose();

                if (modifiable.TryGetCachedComponent<IShieldable>(out IShieldable shieldable))
                {
                    shieldable.OnShieldableDestroyed -= Shieldable_OnDestroyed;
                }

                modifiable.ModifierRemoved -= Modifiable_ModifierRemoved;
            }
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
