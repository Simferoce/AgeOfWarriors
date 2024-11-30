using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk", menuName = "Definition/Technology/Shieldbearer/ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk")]
    public class ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk>
        {
            public Modifier(ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk modifierDefinition) : base(modifierDefinition)
            {
                DeathEventChannel.Instance.Susbribe(OnUnitDeath);
            }

            public override void Dispose()
            {
                base.Dispose();
                DeathEventChannel.Instance.Unsubcribe(OnUnitDeath);
            }

            public void OnUnitDeath(DeathEventChannel.Event evt)
            {
                if (evt.AgentObject.Faction != (modifiable.Entity as AgentObject).Faction)
                {
                    Character character = modifiable.Entity.GetCachedComponent<Character>();

                    foreach (AgentObject agent in AgentObject.All)
                    {
                        if (!agent.IsActive)
                            continue;

                        if (agent == evt.AgentObject)
                            continue;

                        if (agent.Faction == character.Faction)
                            continue;

                        if (!agent.TryGetCachedComponent<Target>(out Target targeteable))
                            continue;

                        if (!agent.TryGetCachedComponent<ModifierHandler>(out ModifierHandler targetModifiable))
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
                            Source.Apply(targetModifiable, new DamageDealtReductionModifierDefinition.Modifier(
                                    definition.damageDealtReductionModifierDefinition,
                                    definition.reductionAmount)
                                    .With(new CharacterModifierTimeElement(definition.duration)));
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
            return string.Format(Description, reductionAmount, StatisticFormatter.Percentage(percentageReachAffect, StatisticDefinition.Reach), duration);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
