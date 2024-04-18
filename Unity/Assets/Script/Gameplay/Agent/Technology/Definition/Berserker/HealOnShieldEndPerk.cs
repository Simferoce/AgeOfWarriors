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
                    shieldable.OnShieldBroken += Shieldable_OnShieldBroken;
                }
            }

            private void Shieldable_OnShieldBroken(Shield shield)
            {
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
                shieldable.OnShieldBroken -= Shieldable_OnShieldBroken;
            }

            public override void Dispose()
            {
                base.Dispose();

                if (modifiable.TryGetCachedComponent<IShieldable>(out IShieldable shieldable))
                {
                    shieldable.OnShieldBroken -= Shieldable_OnShieldBroken;
                    shieldable.OnShieldableDestroyed -= Shieldable_OnDestroyed;
                }
            }
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
