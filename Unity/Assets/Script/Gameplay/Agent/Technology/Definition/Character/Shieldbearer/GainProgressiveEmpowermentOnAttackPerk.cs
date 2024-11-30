using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainProgressiveEmpowermentOnAttackPerk", menuName = "Definition/Technology/Shieldbearer/GainProgressiveEmpowermentOnAttackPerk")]
    public class GainProgressiveEmpowermentOnAttackPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, GainProgressiveEmpowermentOnAttackPerk>
        {
            public Modifier(GainProgressiveEmpowermentOnAttackPerk modifierDefinition) : base(modifierDefinition)
            {

            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                modifiable.Entity.GetCachedComponent<AttackFactory>().OnAttackDealt += Modifier_OnAttackLanded;
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
                    Source.Apply(modifiable, new ProgressiveEmpowermentModifierDefinition.Modifier(definition.modifierToGain));
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.Entity.GetCachedComponent<AttackFactory>().OnAttackDealt -= Modifier_OnAttackLanded;
            }
        }

        [SerializeField] private ProgressiveEmpowermentModifierDefinition modifierToGain;

        public override string ParseDescription()
        {
            return string.Format(Description, modifierToGain.MaxStack, modifierToGain.EmpoweredModifierDefinition.PercentageDamageIncrease);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
