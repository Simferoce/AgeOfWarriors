using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ReduceAttackSpeedPerk", menuName = "Definition/Technology/Seer/ReduceAttackSpeed")]
    public class ReduceAttackSpeedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, ReduceAttackSpeedPerk>
        {
            private List<AttackSpeedReductionModifierDefinition.Modifier> currents = new List<AttackSpeedReductionModifierDefinition.Modifier>();

            public Modifier(IModifiable modifiable, ReduceAttackSpeedPerk modifierDefinition, IModifierSource source = null) : base(modifiable, modifierDefinition, source)
            {
            }

            public override void Update()
            {
                base.Update();

                Character character = modifiable.GetCachedComponent<Character>();

                foreach (AgentObject agent in AgentObject.All)
                {
                    if (agent == character)
                        continue;

                    if (!agent.IsActive)
                        continue;

                    if (!agent.TryGetCachedComponent<ITargeteable>(out ITargeteable targeteable))
                        continue;

                    if (!agent.TryGetCachedComponent<IModifiable>(out IModifiable modifiable))
                        continue;

                    if (character.Faction == targeteable.Faction)
                        continue;

                    if (modifiable.TryGetModifier(definition.attackSpeedReductionModifierDefinition, out _))
                        continue;

                    if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > definition.percentageReach * character.Reach)
                    {
                        AttackSpeedReductionModifierDefinition.Modifier modifierOutOfRange = currents.FirstOrDefault(x => x.Modifiable == modifiable);
                        if (modifierOutOfRange != null)
                        {
                            modifiable.RemoveModifier(modifierOutOfRange);
                            currents.Remove(modifierOutOfRange);
                        }

                        continue;
                    }

                    AttackSpeedReductionModifierDefinition.Modifier modifier = new AttackSpeedReductionModifierDefinition.Modifier(modifiable, definition.attackSpeedReductionModifierDefinition, definition.amount);
                    modifiable.AddModifier(modifier);
                    currents.Add(modifier);
                }
            }
        }

        [SerializeField] private AttackSpeedReductionModifierDefinition attackSpeedReductionModifierDefinition;
        [SerializeField, Range(0, 5)] private float percentageReach;
        [SerializeField, Range(0, 1)] private float amount;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }

        public override string ParseDescription()
        {
            return string.Format(Description, amount, StatisticFormatter.Percentage(percentageReach, StatisticDefinition.Reach));
        }
    }
}
