using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainProgressiveEmpowermentOnAttackPerk", menuName = "Definition/Technology/Shieldbearer/GainProgressiveEmpowermentOnAttackPerk")]
    public class GainProgressiveEmpowermentOnAttackPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private ProgressiveEmpowermentModifierDefinition modifierToGain;

            public override bool TryGetValue<T>(StatisticDefinition definition, out T value)
            {
                if (definition == StatisticDefinition.Stack)
                {
                    value = modifierToGain.GetValueOrThrow<T>(this, definition);
                    return true;
                }

                return base.TryGetValue(definition, out value);
            }

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, ProgressiveEmpowermentModifierDefinition modifierToGain) : base(modifiable, modifierDefinition)
            {
                this.modifierToGain = modifierToGain;

                modifiable.GetCachedComponent<Character>().OnAttackLanded += Modifier_OnAttackLanded;
            }

            private void Modifier_OnAttackLanded(Attack attack, float damageDealt, bool killingBlow)
            {
                Game.Modifier modifier = modifiable.GetModifiers().FirstOrDefault(x => x.Definition == modifierToGain);
                if (modifier != null)
                {
                    modifier.Refresh();
                }
                else
                {
                    modifiable.AddModifier(new ProgressiveEmpowermentModifierDefinition.Modifier(modifiable, modifierToGain));
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.GetCachedComponent<Character>().OnAttackLanded -= Modifier_OnAttackLanded;
            }
        }

        [SerializeField] private ProgressiveEmpowermentModifierDefinition modifierToGain;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifierToGain);
        }
    }
}
