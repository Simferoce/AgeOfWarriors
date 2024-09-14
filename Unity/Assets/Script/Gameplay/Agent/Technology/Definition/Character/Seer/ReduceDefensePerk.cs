using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ReduceDefensePerk", menuName = "Definition/Technology/Seer/ReduceDefensePerk")]
    public class ReduceDefensePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, ReduceDefensePerk>
        {
            public Modifier(ModifierHandler modifiable, ReduceDefensePerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                modifiable.Entity.GetCachedComponent<Character>().OnAttackLanded += Modifier_OnAttackLanded;
            }

            private void Modifier_OnAttackLanded(AttackResult attack)
            {
                Character character = modifiable.Entity.GetCachedComponent<Character>();

                ModifierHandler targetModifiable = attack.Target.Entity.GetCachedComponent<ModifierHandler>();
                Game.Modifier modifier = targetModifiable.GetModifiers().FirstOrDefault(x => x is DefenseReductionModifierDefinition.Modifier && x.Source == (IModifierSource)character);
                if (modifier != null)
                {
                    modifier.Refresh();
                }
                else
                {
                    targetModifiable.AddModifier(new DefenseReductionModifierDefinition.Modifier(targetModifiable, definition.defenseReductionModifierDefinition, definition.duration, definition.defenseReduction, character));
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.Entity.GetCachedComponent<Character>().OnAttackLanded -= Modifier_OnAttackLanded;
            }
        }

        [SerializeField] private DefenseReductionModifierDefinition defenseReductionModifierDefinition;
        [SerializeField] private float defenseReduction;
        [SerializeField] private float duration;

        public override string ParseDescription()
        {
            return string.Format(Description, defenseReduction, duration);
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
