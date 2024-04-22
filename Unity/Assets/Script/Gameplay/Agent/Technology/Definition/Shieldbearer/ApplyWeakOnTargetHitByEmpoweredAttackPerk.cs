using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ApplyWeakOnTargetHitByEmpoweredAttackPerk", menuName = "Definition/Technology/Shieldbearer/ApplyWeakOnTargetHitByEmpoweredAttackPerk")]
    public class ApplyWeakOnTargetHitByEmpoweredAttackPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, ApplyWeakOnTargetHitByEmpoweredAttackPerk>
        {
            public Modifier(IModifiable modifiable, ApplyWeakOnTargetHitByEmpoweredAttackPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
                modifiable.GetCachedComponent<Character>().OnAttackLanded += Modifier_OnAttackLanded;
            }

            private void Modifier_OnAttackLanded(AttackResult attack)
            {
                if (attack.Attack.Empowered)
                {
                    IModifiable targetModifiable = attack.Target.GetCachedComponent<IModifiable>();
                    Game.Modifier modifier = targetModifiable.GetModifiers().FirstOrDefault(x => x.Definition == definition.damageDealtReductionModifierDefinition);
                    if (modifier != null)
                    {
                        modifier.Refresh();
                    }
                    else
                    {
                        targetModifiable.AddModifier(
                            new DamageDealtReductionModifierDefinition.Modifier(targetModifiable, definition.damageDealtReductionModifierDefinition, definition.damageReduction)
                            .With(new CharacterModifierTimeElement(definition.duration)));
                    }
                }

            }

            public override void Dispose()
            {
                base.Dispose();

                modifiable.GetCachedComponent<Character>().OnAttackLanded += Modifier_OnAttackLanded;
            }
        }

        [SerializeField, Range(0, 5)] private float damageReduction;
        [SerializeField] private float duration;
        [SerializeField] private DamageDealtReductionModifierDefinition damageDealtReductionModifierDefinition;

        public override string ParseDescription()
        {
            return $"Whenever an empowered attack lands, apply weak on the target for {duration} " +
                $"seconds which reduce the damage dealt of {damageReduction}.";
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
