using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BleedingModifierDefinition", menuName = "Definition/Modifier/BleedingModifierDefinition")]
    public class BleedingModifierDefinition : DamageOverTimeModifierDefinition
    {
        public class BleedingModifier : Modifier
        {
            public int Stacks { get; set; }

            public bool IsMaxed => Stacks >= (definition as BleedingModifierDefinition).maxStack;

            public BleedingModifier(BleedingModifierDefinition modifierDefinition, float duration, float damageOverTime) : base(modifierDefinition, duration, damageOverTime)
            {
                Stacks++;
            }

            public override float? GetStack()
            {
                return Stacks;
            }

            public void Increase(float damagePerSeconds, float duration, bool spread, float spreadDistance, ModifierApplier source)
            {
                if (!IsMaxed)
                {
                    Stacks++;
                    DamagePerSeconds = damagePerSeconds;
                }
                else if (spread)
                {
                    Character character = modifiable.Entity.GetCachedComponent<Character>();

                    foreach (AgentObject agent in AgentObject.All)
                    {
                        if (agent == character)
                            continue;

                        if (!agent.IsActive)
                            continue;

                        if (agent.Faction != character.Faction)
                            continue;

                        if (!agent.TryGetCachedComponent<Target>(out Target targeteable))
                            continue;

                        if (!agent.TryGetCachedComponent<ModifierHandler>(out ModifierHandler modifiable))
                            continue;

                        if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > spreadDistance)
                            continue;

                        BleedingModifier modifier = modifiable.GetModifiers()
                            .FirstOrDefault(x => x is BleedingModifier bleedingModifier
                                && bleedingModifier.Source == source)
                            as BleedingModifier;

                        if (modifier == null)
                        {
                            modifier = new BleedingModifier(definition as BleedingModifierDefinition, duration, damagePerSeconds);
                            Source.Apply(modifiable, modifier, new List<ModifierParameter>());
                        }
                        else
                        {
                            SpreadBleedingPerk.Modifier spreadModifier = modifiable.GetModifiers().FirstOrDefault(x => x is SpreadBleedingPerk.Modifier) as SpreadBleedingPerk.Modifier;
                            modifier.Increase(damagePerSeconds, duration, spreadModifier != null, spreadModifier?.SpreadDistance ?? 0f, source);
                        }

                        break;
                    }
                }

                Refresh();
            }
        }

        [SerializeField] private int maxStack;

        public int MaxStack { get => maxStack; set => maxStack = value; }
    }
}
