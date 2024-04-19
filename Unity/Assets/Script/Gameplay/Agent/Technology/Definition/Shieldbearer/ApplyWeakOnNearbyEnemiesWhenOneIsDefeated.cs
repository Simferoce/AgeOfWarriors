using System;
using UnityEngine;

namespace Game
{
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
                if (evt.AgentObject.Faction != (modifiable as ITargeteable).Faction)
                {
                    Character character = modifiable.GetCachedComponent<Character>();

                    foreach (AgentObject agent in AgentObject.All)
                    {
                        if (!agent.IsActive)
                            continue;

                        if (!agent.TryGetCachedComponent<ITargeteable>(out ITargeteable targeteable))
                            continue;

                        if (!agent.TryGetCachedComponent<IModifiable>(out IModifiable targetModifiable))
                            continue;

                        if (Mathf.Abs((targeteable.ClosestPoint(character.CenterPosition) - character.CenterPosition).x) > Definition.GetValueOrThrow<float>(this, StatisticDefinition.Range))
                            continue;

                        modifiable.AddModifier(
                            new DamageDealtReductionModifierDefinition.Modifier(targetModifiable, damageDealtReductionModifierDefinition)
                            .With(new CharacterModifierTimeElement(Definition.GetValueOrThrow<float>(this, StatisticDefinition.BuffDuration)))
                            );
                    }
                }
            }
        }

        [SerializeField] private DamageDealtReductionModifierDefinition damageDealtReductionModifierDefinition;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            throw new NotImplementedException();
        }
    }
}
