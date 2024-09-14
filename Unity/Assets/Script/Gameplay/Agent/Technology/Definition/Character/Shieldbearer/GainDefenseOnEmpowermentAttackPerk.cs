using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainDefenseOnEmpowermentAttackPerk", menuName = "Definition/Technology/Shieldbearer/GainDefenseOnEmpowermentAttackPerk")]
    public class GainDefenseOnEmpowermentAttackPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GainDefenseOnEmpowermentAttackPerk>
        {
            public Modifier(ModifierHandler modifiable, GainDefenseOnEmpowermentAttackPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                modifiable.Entity.GetCachedComponent<Character>().OnAttackLanded += Modifier_OnAttackLanded;
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
                        modifiable.AddModifier(
                            new DefenseModifierDefinition.Modifier(
                                modifiable.Entity.GetCachedComponent<Character>(),
                                definition.defenseModifierDefinition,
                                definition.defense,
                                Source)
                            .With(new CharacterModifierTimeElement(definition.buffDuration)));
                    }
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                modifiable.Entity.GetCachedComponent<Character>().OnAttackLanded += Modifier_OnAttackLanded;
            }
        }

        [SerializeField] private float buffDuration;
        [SerializeField] private float defense;
        [SerializeField] private DefenseModifierDefinition defenseModifierDefinition;

        public override string ParseDescription()
        {
            return string.Format(Description, defense, buffDuration);
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
