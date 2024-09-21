using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk", menuName = "Definition/Technology/Shieldbearer/ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk")]
    public class ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk>
        {
            public Modifier(ModifierHandler modifiable, ApplyWeakOnNearbyEnemiesWhenOneIsDefeatedPerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
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

                        if (Mathf.Abs((targeteable.ClosestPoint(character.GetCachedComponent<Target>().CenterPosition) - character.GetCachedComponent<Target>().CenterPosition).x) > definition.percentageReachAffect * character.Reach)
                            continue;

                        Game.Modifier modifier = targetModifiable.GetModifiers().FirstOrDefault(x => x.Definition == definition.damageDealtReductionModifierDefinition);
                        if (modifier != null)
                        {
                            modifier.Refresh();
                        }
                        else
                        {
                            targetModifiable.AddModifier(
                                new DamageDealtReductionModifierDefinition.Modifier(
                                    targetModifiable,
                                    definition.damageDealtReductionModifierDefinition,
                                    definition.reductionAmount,
                                    Source)
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

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
