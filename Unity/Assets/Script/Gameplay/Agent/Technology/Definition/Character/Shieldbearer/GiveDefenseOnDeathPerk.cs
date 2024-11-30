using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GiveDefenseOnDeathPerk", menuName = "Definition/Technology/Shieldbearer/GiveDefenseOnDeathPerk")]
    public class GiveDefenseOnDeathPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, GiveDefenseOnDeathPerk>
        {
            public Modifier(GiveDefenseOnDeathPerk modifierDefinition) : base(modifierDefinition)
            {
            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                modifiable.Entity.GetCachedComponent<Character>().OnDeath += Modifier_OnDeath;
            }

            private void Modifier_OnDeath()
            {
                modifiable.Entity.GetCachedComponent<Character>().OnDeath -= Modifier_OnDeath;

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

                    if (agent.Faction != character.Faction)
                        continue;

                    if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > definition.reachPercentage * character.Reach)
                        continue;

                    Source.Apply(modifiable, definition.statisticModifierDefinition.Instantiate()
                            .With(new CharacterModifierTimeElement(definition.duration)),
                            new List<ModifierParameter>() { new ModifierParameter<float>("value", definition.defense), new ModifierParameter<StatisticDefinition>("definition", StatisticDefinition.FlatDefense) });
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.Entity.GetCachedComponent<Character>().OnDeath -= Modifier_OnDeath;
            }
        }

        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField] private float defense;
        [SerializeField] private float duration;
        [SerializeField] private GiveDefenseOnDeathModifier statisticModifierDefinition;

        public override string ParseDescription()
        {
            return string.Format(Description, defense, StatisticFormatter.Percentage(reachPercentage, StatisticDefinition.Reach));
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
