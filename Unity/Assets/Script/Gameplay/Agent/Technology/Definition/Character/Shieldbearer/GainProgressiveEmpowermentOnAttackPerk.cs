using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainProgressiveEmpowermentOnAttackPerk", menuName = "Definition/Technology/Shieldbearer/GainProgressiveEmpowermentOnAttackPerk")]
    public class GainProgressiveEmpowermentOnAttackPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GainProgressiveEmpowermentOnAttackPerk>
        {
            public Modifier(ModifierHandler modifiable, GainProgressiveEmpowermentOnAttackPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                modifiable.Entity.GetCachedComponent<Character>().OnAttackLanded += Modifier_OnAttackLanded;
            }

            private void Modifier_OnAttackLanded(AttackResult attackResult)
            {
                if (attackResult.Attack.Empowered)
                    return;

                Game.Modifier modifier = modifiable.GetModifiers().FirstOrDefault(x => x.Definition == definition.modifierToGain);
                if (modifier != null)
                {
                    modifier.Refresh();
                }
                else
                {
                    modifiable.AddModifier(new ProgressiveEmpowermentModifierDefinition.Modifier(
                        modifiable,
                        definition.modifierToGain,
                        Source));
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.Entity.GetCachedComponent<Character>().OnAttackLanded -= Modifier_OnAttackLanded;
            }
        }

        [SerializeField] private ProgressiveEmpowermentModifierDefinition modifierToGain;

        public override string ParseDescription()
        {
            return string.Format(Description, modifierToGain.MaxStack, modifierToGain.EmpoweredModifierDefinition.PercentageDamageIncrease);
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
