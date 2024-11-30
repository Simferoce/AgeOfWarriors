using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ReduceDefensePerk", menuName = "Definition/Technology/Seer/ReduceDefensePerk")]
    public class ReduceDefensePerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ReduceDefensePerk>
        {
            public Modifier(ReduceDefensePerk modifierDefinition) : base(modifierDefinition)
            {
            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);

                modifiable.Entity.GetCachedComponent<AttackFactory>().OnAttackDealt += Modifier_OnAttackLanded;
            }

            private void Modifier_OnAttackLanded(AttackResult attack)
            {
                Character character = modifiable.Entity.GetCachedComponent<Character>();

                ModifierHandler targetModifiable = attack.Target.Entity.GetCachedComponent<ModifierHandler>();
                Game.Modifier modifier = targetModifiable.GetModifiers().FirstOrDefault(x => x is DefenseReductionModifierDefinition.Modifier && x.Source == character.GetCachedComponent<ModifierApplier>());
                if (modifier != null)
                {
                    modifier.Refresh();
                }
                else
                {
                    Source.Apply(targetModifiable, new DefenseReductionModifierDefinition.Modifier(definition.defenseReductionModifierDefinition, definition.duration, definition.defenseReduction));
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.Entity.GetCachedComponent<AttackFactory>().OnAttackDealt -= Modifier_OnAttackLanded;
            }
        }

        [SerializeField] private DefenseReductionModifierDefinition defenseReductionModifierDefinition;
        [SerializeField] private float defenseReduction;
        [SerializeField] private float duration;

        public override string ParseDescription()
        {
            return string.Format(Description, defenseReduction, duration);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
