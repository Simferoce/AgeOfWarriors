using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ApplyWeakOnNearbyEnemiesWhenOneIsDefeated", menuName = "Definition/Technology/Shieldbearer/ApplyWeakOnNearbyEnemiesWhenOneIsDefeated")]
    public class ApplyWeakOnNearbyEnemiesWhenOneIsDefeated : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private DamageDealtReductionModifierDefinition damageDealtReductionModifierDefinition;

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, DamageDealtReductionModifierDefinition damageDealtReductionModifierDefinition) : base(modifiable, modifierDefinition)
            {
                EventChannelDeath.Instance.Susbribe(OnUnitDeath);
                this.damageDealtReductionModifierDefinition = damageDealtReductionModifierDefinition;
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

                        if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > Definition.GetValueOrThrow<float>(this, StatisticDefinition.Range))
                            continue;

                        Game.Modifier modifier = targetModifiable.GetModifiers().FirstOrDefault(x => x.Definition == damageDealtReductionModifierDefinition);
                        if (modifier != null)
                        {
                            modifier.Refresh();
                        }
                        else
                        {
                            targetModifiable.AddModifier(
                                new DamageDealtReductionModifierDefinition.Modifier(targetModifiable, damageDealtReductionModifierDefinition)
                                    .With(new CharacterModifierTimeElement(Definition.GetValueOrThrow<float>(this, StatisticDefinition.BuffDuration)))
                            );
                        }
                    }
                }
            }
        }

        [SerializeField] private DamageDealtReductionModifierDefinition damageDealtReductionModifierDefinition;

        public override string ParseDescription(object caller, string description)
        {
            description = base.ParseDescription(caller, description);
            description = damageDealtReductionModifierDefinition.ParseDescription(caller, description);

            return description;
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, damageDealtReductionModifierDefinition);
        }
    }
}
