using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ApplyWeakOnTargetHitByEmpoweredAttackPerk", menuName = "Definition/Technology/Shieldbearer/ApplyWeakOnTargetHitByEmpoweredAttackPerk")]
    public class ApplyWeakOnTargetHitByEmpoweredAttackPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private DamageDealtReductionModifierDefinition damageDealtReductionModifierDefinition;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, DamageDealtReductionModifierDefinition damageDealtReductionModifierDefinition) : base(modifiable, modifierDefinition)
            {
                modifiable.GetCachedComponent<Character>().OnAttackLanded += Modifier_OnAttackLanded;
                this.damageDealtReductionModifierDefinition = damageDealtReductionModifierDefinition;
            }

            private void Modifier_OnAttackLanded(AttackResult attack)
            {
                if (attack.Attack.Empowered)
                {
                    IModifiable targetModifiable = attack.Target.GetCachedComponent<IModifiable>();
                    Game.Modifier modifier = targetModifiable.GetModifiers().FirstOrDefault(x => x.Definition == damageDealtReductionModifierDefinition);
                    if (modifier != null)
                    {
                        modifier.Refresh();
                    }
                    else
                    {
                        targetModifiable.AddModifier(
                            new DamageDealtReductionModifierDefinition.Modifier(targetModifiable, damageDealtReductionModifierDefinition)
                            .With(new CharacterModifierTimeElement(Definition.GetValueOrThrow<float>(this, StatisticDefinition.BuffDuration))));
                    }
                }

            }

            public override void Dispose()
            {
                base.Dispose();

                modifiable.GetCachedComponent<Character>().OnAttackLanded += Modifier_OnAttackLanded;
            }
        }

        [SerializeField] private DamageDealtReductionModifierDefinition damageDealtReductionModifierDefinition;

        public override string ParseDescription(object caller, string description)
        {
            description = base.ParseDescription(caller, description);
            description = damageDealtReductionModifierDefinition.ParseDescription(caller, description);

            return description;
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, damageDealtReductionModifierDefinition);
        }
    }
}
