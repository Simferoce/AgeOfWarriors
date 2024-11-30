using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ReduceAttackSpeedPerk", menuName = "Definition/Technology/Seer/ReduceAttackSpeed")]
    public class ReduceAttackSpeedPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ReduceAttackSpeedPerk>
        {
            private List<AttackSpeedModifierDefinition.Modifier> currents = new List<AttackSpeedModifierDefinition.Modifier>();

            public Modifier(ReduceAttackSpeedPerk modifierDefinition) : base(modifierDefinition)
            {
            }

            public override void Update()
            {
                base.Update();

                Character character = modifiable.Entity.GetCachedComponent<Character>();

                foreach (AgentObject agent in AgentObject.All)
                {
                    if (agent == character)
                        continue;

                    if (!agent.IsActive)
                        continue;

                    if (!agent.TryGetCachedComponent<Target>(out Target targeteable))
                        continue;

                    if (!agent.TryGetCachedComponent<ModifierHandler>(out ModifierHandler modifiable))
                        continue;

                    if (character.Faction == (targeteable.Entity as AgentObject).Faction)
                        continue;

                    if (modifiable.TryGetModifier(definition.attackSpeedReductionModifierDefinition, out _))
                        continue;

                    if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > definition.percentageReach * character.Reach)
                    {
                        AttackSpeedModifierDefinition.Modifier modifierOutOfRange = currents.FirstOrDefault(x => x.Modifiable == modifiable);
                        if (modifierOutOfRange != null)
                        {
                            modifiable.RemoveModifier(modifierOutOfRange);
                            currents.Remove(modifierOutOfRange);
                        }

                        continue;
                    }

                    AttackSpeedModifierDefinition.Modifier modifier = new AttackSpeedModifierDefinition.Modifier(
                        definition.attackSpeedReductionModifierDefinition,
                        -definition.amount);

                    Source.Apply(modifiable, modifier);

                    currents.Add(modifier);
                }
            }
        }

        [SerializeField] private AttackSpeedModifierDefinition attackSpeedReductionModifierDefinition;
        [SerializeField, Range(0, 5)] private float percentageReach;
        [SerializeField, Range(0, 1)] private float amount;

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }

        public override string ParseDescription()
        {
            return string.Format(Description, amount, StatisticFormatter.Percentage(percentageReach, StatisticDefinition.Reach));
        }
    }
}
