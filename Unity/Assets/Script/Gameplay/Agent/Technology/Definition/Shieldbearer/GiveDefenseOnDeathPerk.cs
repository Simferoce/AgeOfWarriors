using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GiveDefenseOnDeathPerk", menuName = "Definition/Technology/Shieldbearer/GiveDefenseOnDeathPerk")]
    public class GiveDefenseOnDeathPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GiveDefenseOnDeathPerk>
        {
            public Modifier(IModifiable modifiable, GiveDefenseOnDeathPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
                modifiable.GetCachedComponent<Character>().OnDeath += Modifier_OnDeath;
            }

            private void Modifier_OnDeath()
            {
                modifiable.GetCachedComponent<Character>().OnDeath -= Modifier_OnDeath;

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

                    if (agent.Faction != character.Faction)
                        continue;

                    if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > definition.reachPercentage * character.Reach)
                        continue;

                    modifiable.AddModifier(new DefenseModifierDefinition.Modifier(character, definition.defenseModifierDefinition, definition.defense, this));
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.GetCachedComponent<Character>().OnDeath -= Modifier_OnDeath;
            }
        }

        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField] private float defense;
        [SerializeField] private DefenseModifierDefinition defenseModifierDefinition;

        public override string ParseDescription()
        {
            return $"Increase defense by {defense} for every allies in {StatisticFormatter.Percentage(reachPercentage, StatisticDefinition.Reach)} meters whenever the unit dies.";
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
