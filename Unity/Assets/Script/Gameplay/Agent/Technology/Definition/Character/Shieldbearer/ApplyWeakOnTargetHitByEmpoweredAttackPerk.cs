using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ApplyWeakOnTargetHitByEmpoweredAttackPerk", menuName = "Definition/Technology/Shieldbearer/ApplyWeakOnTargetHitByEmpoweredAttackPerk")]
    public class ApplyWeakOnTargetHitByEmpoweredAttackPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ApplyWeakOnTargetHitByEmpoweredAttackPerk>
        {
            public Modifier(ApplyWeakOnTargetHitByEmpoweredAttackPerk modifierDefinition) : base(modifierDefinition)
            {

            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                modifiable.Entity.GetCachedComponent<AttackFactory>().OnAttackDealt += Modifier_OnAttackLanded;
            }

            private void Modifier_OnAttackLanded(AttackResult attack)
            {
                if (attack.Attack.Empowered)
                {
                    ModifierHandler targetModifiable = attack.Target.Entity.GetCachedComponent<ModifierHandler>();
                    Game.Modifier modifier = targetModifiable.GetModifiers().FirstOrDefault(x => x.Definition == definition.damageDealtReductionModifierDefinition);
                    if (modifier != null)
                    {
                        modifier.Refresh();
                    }
                    else
                    {
                        Source.Apply(modifiable,
                            new DamageDealtReductionModifierDefinition.Modifier(
                                definition.damageDealtReductionModifierDefinition,
                                definition.damageReduction)
                            .With(new CharacterModifierTimeElement(definition.duration)));
                    }
                }

            }

            public override void Dispose()
            {
                base.Dispose();

                modifiable.Entity.GetCachedComponent<AttackFactory>().OnAttackDealt += Modifier_OnAttackLanded;
            }
        }

        [SerializeField, Range(0, 5)] private float damageReduction;
        [SerializeField] private float duration;
        [SerializeField] private DamageDealtReductionModifierDefinition damageDealtReductionModifierDefinition;

        public override string ParseDescription()
        {
            return string.Format(Description, duration, damageReduction);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
