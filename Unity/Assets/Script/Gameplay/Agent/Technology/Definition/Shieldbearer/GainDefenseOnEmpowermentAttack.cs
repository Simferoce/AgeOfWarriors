using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainDefenseOnEmpowermentAttack", menuName = "Definition/Technology/Shieldbearer/GainDefenseOnEmpowermentAttack")]
    public class GainDefenseOnEmpowermentAttack : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GainDefenseOnEmpowermentAttack>
        {
            public Modifier(IModifiable modifiable, GainDefenseOnEmpowermentAttack modifierDefinition) : base(modifiable, modifierDefinition)
            {
                modifiable.GetCachedComponent<Character>().OnAttackLanded += Modifier_OnAttackLanded;
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
                            new DefenseModifierDefinition.Modifier(modifiable.GetCachedComponent<Character>(), definition.defenseModifierDefinition, definition.defense, this)
                            .With(new CharacterModifierTimeElement(definition.buffDuration)));
                    }
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                modifiable.GetCachedComponent<Character>().OnAttackLanded += Modifier_OnAttackLanded;
            }
        }

        [SerializeField] private float buffDuration;
        [SerializeField] private float defense;
        [SerializeField] private DefenseModifierDefinition defenseModifierDefinition;

        public override string ParseDescription()
        {
            return $"Increase defense by {defense} for {buffDuration} seconds each time an empowerment attack lands";
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
