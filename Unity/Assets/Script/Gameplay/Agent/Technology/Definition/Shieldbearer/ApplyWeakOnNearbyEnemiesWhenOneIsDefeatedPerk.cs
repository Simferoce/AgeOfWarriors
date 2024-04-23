using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk", menuName = "Definition/Technology/Shieldbearer/ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk")]
    public class ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk>
        {
            public Modifier(IModifiable modifiable, ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
                EventChannelDeath.Instance.Susbribe(OnUnitDeath);
            }

            public override void Dispose()
            {
                base.Dispose();
                EventChannelDeath.Instance.Unsubcribe(OnUnitDeath);
            }

            public void OnUnitDeath(EventChannelDeath.Event evt)
            {
                if (evt.AgentObject.Faction != modifiable.GetCachedComponent<ITargeteable>().Faction)
                {
                    Character character = modifiable.GetCachedComponent<Character>();

                    foreach (AgentObject agent in AgentObject.All)
                    {
                        if (!agent.IsActive)
                            continue;

                        if (agent == evt.AgentObject)
                            continue;

                        if (agent.Faction == character.Faction)
                            continue;

                        if (!agent.TryGetCachedComponent<ITargeteable>(out ITargeteable targeteable))
                            continue;

                        if (!agent.TryGetCachedComponent<IModifiable>(out IModifiable targetModifiable))
                            continue;

                        if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > definition.percentageReachAffect * character.Reach)
                            continue;

                        Game.Modifier modifier = targetModifiable.GetModifiers().FirstOrDefault(x => x.Definition == definition.damageDealtReductionModifierDefinition);
                        if (modifier != null)
                        {
                            modifier.Refresh();
                        }
                        else
                        {
                            targetModifiable.AddModifier(
                                new DamageDealtReductionModifierDefinition.Modifier(targetModifiable, definition.damageDealtReductionModifierDefinition, definition.reductionAmount)
                                    .With(new CharacterModifierTimeElement(definition.duration))
                            );
                        }
                    }
                }
            }
        }

        [SerializeField] private float duration;
        [SerializeField, Range(0, 5)] private float percentageReachAffect;
        [SerializeField, Range(0, 5)] private float reductionAmount;
        [SerializeField] private DamageDealtReductionModifierDefinition damageDealtReductionModifierDefinition;

        public override string ParseDescription()
        {
            return $"Whenever an enemy unit die, apply weak which reduce the damage dealt by " +
                $"{reductionAmount:0.0%} of every enemy unit in {StatisticFormatter.Percentage(percentageReachAffect, StatisticDefinition.Reach)} meters for {duration} seconds.";
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
