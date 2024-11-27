using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GiveDefenseOnDeathPerk", menuName = "Definition/Technology/Shieldbearer/GiveDefenseOnDeathPerk")]
    public class GiveDefenseOnDeathPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GiveDefenseOnDeathPerk>
        {
            public Modifier(ModifierHandler modifiable, GiveDefenseOnDeathPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
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

                    if (!agent.TryGetCachedComponent<ITargeteable>(out ITargeteable targeteable))
                        continue;

                    if (!agent.TryGetCachedComponent<ModifierHandler>(out ModifierHandler modifiable))
                        continue;

                    if (agent.Faction != character.Faction)
                        continue;

                    if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > definition.reachPercentage * character.Reach)
                        continue;

                    modifiable.AddModifier(new DefenseModifierDefinition.Modifier(character,
                        definition.defenseModifierDefinition,
                        definition.defense,
                        Source));
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
        [SerializeField] private DefenseModifierDefinition defenseModifierDefinition;

        public override string ParseDescription()
        {
            return string.Format(Description, defense, StatisticFormatter.Percentage(reachPercentage, StatisticDefinition.Reach));
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
