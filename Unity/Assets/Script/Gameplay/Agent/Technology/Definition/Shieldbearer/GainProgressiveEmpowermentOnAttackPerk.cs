using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainProgressiveEmpowermentOnAttackPerk", menuName = "Definition/Technology/Shieldbearer/GainProgressiveEmpowermentOnAttackPerk")]
    public class GainProgressiveEmpowermentOnAttackPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GainProgressiveEmpowermentOnAttackPerk>
        {
            public Modifier(IModifiable modifiable, GainProgressiveEmpowermentOnAttackPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
                modifiable.GetCachedComponent<Character>().OnAttackLanded += Modifier_OnAttackLanded;
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
                    modifiable.AddModifier(new ProgressiveEmpowermentModifierDefinition.Modifier(modifiable, definition.modifierToGain));
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.GetCachedComponent<Character>().OnAttackLanded -= Modifier_OnAttackLanded;
            }
        }

        [SerializeField] private ProgressiveEmpowermentModifierDefinition modifierToGain;

        public override string ParseDescription(object caller, string description)
        {
            description = base.ParseDescription(caller, description);
            description = modifierToGain.ParseDescription(caller, description);

            return description;
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
