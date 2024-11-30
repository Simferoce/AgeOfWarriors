using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SeerMadnessPerk", menuName = "Definition/Technology/Seer/SeerMadnessPerk")]
    public class SeerMadnessPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, SeerMadnessPerk>
        {
            private int currentAttackApplied = 0;
            private Ability affectedAbility;
            private Character character;
            private Statistic<float> attackPowerFlat;

            public Modifier(SeerMadnessPerk modifierDefinition) : base(modifierDefinition)
            {

            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                character = modifiable.Entity.GetCachedComponent<Character>();
                affectedAbility = character.GetCachedComponent<Caster>().Abilities.FirstOrDefault(x => x.Definition == definition.affectedAbility);
                affectedAbility.OnAbilityEffectApplied += AffectedAbility_OnAbilityEffectApplied;
                attackPowerFlat = new Statistic<float>(StatisticDefinition.FlatAttackPower, definition.attackPower);
                StatisticRegistry.Register(attackPowerFlat);
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
                    Source.Apply(modifiable, new ConfusionModifierDefinition.Modifier(
                            definition.confusionModifierDefinition)
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
                StatisticRegistry.Unregister(attackPowerFlat);
            }
        }

        [SerializeField] private int stack;
        [SerializeField] private float duration;
        [SerializeField] private float attackPower;
        [SerializeField] private ConfusionModifierDefinition confusionModifierDefinition;
        [SerializeField] private AbilityDefinition affectedAbility;

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }

        public override string ParseDescription()
        {
            return string.Format(Description, stack, duration, attackPower);
        }
    }
}
