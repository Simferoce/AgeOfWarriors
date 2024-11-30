using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainDefenseOnEmpowermentAttackPerk", menuName = "Definition/Technology/Shieldbearer/GainDefenseOnEmpowermentAttackPerk")]
    public class GainDefenseOnEmpowermentAttackPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, GainDefenseOnEmpowermentAttackPerk>
        {
            public Modifier(GainDefenseOnEmpowermentAttackPerk modifierDefinition) : base(modifierDefinition)
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
                    Game.Modifier modifier = modifiable.GetModifiers().FirstOrDefault(x => x.Definition == definition.defenseModifierDefinition);
                    if (modifier != null)
                    {
                        modifier.Refresh();
                    }
                    else
                    {
                        Source.Apply(modifiable, definition.defenseModifierDefinition.Instantiate()
                            .With(new CharacterModifierTimeElement(definition.buffDuration)),
                            new List<ModifierParameter>() { new ModifierParameter<float>("value", definition.defense), new ModifierParameter<StatisticDefinition>("definition", StatisticDefinition.FlatDefense) });
                    }
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                modifiable.Entity.GetCachedComponent<AttackFactory>().OnAttackDealt += Modifier_OnAttackLanded;
            }
        }

        [SerializeField] private float buffDuration;
        [SerializeField] private float defense;
        [SerializeField] private StatisticModifierDefinition defenseModifierDefinition;

        public override string ParseDescription()
        {
            return string.Format(Description, defense, buffDuration);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
