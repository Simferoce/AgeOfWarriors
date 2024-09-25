using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SeerMadnessPerk", menuName = "Definition/Technology/Seer/SeerMadnessPerk")]
    public class SeerMadnessPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, SeerMadnessPerk>
        {
            private int currentAttackApplied = 0;
            private Ability affectedAbility;
            private Character character;

            //public override float? AttackPower => definition.attackPower;

            public Modifier(ModifierHandler modifiable, SeerMadnessPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                character = modifiable.Entity.GetCachedComponent<Character>();
                affectedAbility = character.Entity.GetCachedComponent<Caster>().Abilities.FirstOrDefault(x => x.Definition == definition.affectedAbility);
                affectedAbility.OnAbilityEffectApplied += AffectedAbility_OnAbilityEffectApplied;
            }

            private void AffectedAbility_OnAbilityEffectApplied()
            {
                if (character.IsConfused)
                    return;

                if (currentAttackApplied < definition.stack)
                {
                    currentAttackApplied++;
                }
                else
                {
                    modifiable.AddModifier(new ConfusionModifierDefinition.Modifier(
                            modifiable,
                            definition.confusionModifierDefinition,
                            Source)
                        .With(new CharacterModifierTimeElement(definition.duration)));

                    currentAttackApplied = 0;
                }
            }

            public override float? GetStack()
            {
                return currentAttackApplied;
            }

            public override void Dispose()
            {
                base.Dispose();
                affectedAbility.OnAbilityEffectApplied -= AffectedAbility_OnAbilityEffectApplied;
            }
        }

        [SerializeField] private int stack;
        [SerializeField] private float duration;
        [SerializeField] private float attackPower;
        [SerializeField] private ConfusionModifierDefinition confusionModifierDefinition;
        [SerializeField] private AbilityDefinition affectedAbility;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
