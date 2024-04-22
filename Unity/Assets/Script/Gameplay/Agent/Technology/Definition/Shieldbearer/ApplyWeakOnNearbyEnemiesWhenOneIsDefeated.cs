using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ApplyWeakOnNearbyEnemiesWhenOneIsDefeated", menuName = "Definition/Technology/Shieldbearer/ApplyWeakOnNearbyEnemiesWhenOneIsDefeated")]
    public class ApplyWeakOnNearbyEnemiesWhenOneIsDefeated : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, ApplyWeakOnNearbyEnemiesWhenOneIsDefeated>
        {
            public Modifier(IModifiable modifiable, ApplyWeakOnNearbyEnemiesWhenOneIsDefeated modifierDefinition) : base(modifiable, modifierDefinition)
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

                        if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > definition.Range(this))
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

        [Statistic("buff_duration")] public float Duration(Modifier modifier) => duration;
        [Statistic("range", nameof(RangeFormat))] public float Range(Modifier modifier) => (modifier.Modifiable as Character).Reach * percentageReachAffect;
        [Statistic("damage_dealt_reduction", nameof(DamageDealtReductionFormat))] public float DamageDealtReduction(Modifier modifier) => reductionAmount;

        public string RangeFormat(Modifier modifier) => StatisticFormatter.Percentage<Modifier>(Range, percentageReachAffect, StatisticDefinition.Reach, modifier);
        public string DamageDealtReductionFormat(Modifier modifier) => reductionAmount.ToString("0.0%");

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
