using Game.Agent;
using Game.Components;
using Game.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Modifier
{
    [Serializable]
    public class ApplyBloodModifierEffect : ModifierEffect
    {
        [SerializeField] private ModifierDefinition definition;
        [SerializeReference, SubclassSelector] private List<ModifierTarget> targets;
        [SerializeReference, SubclassSelector] private List<ModifierParameterFactory> parameterFactories;
        [SerializeField] private ModifierDefinition spreadBloodPerk;

        private ModifierApplier modifierApplier;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);

            modifierApplier = modifier.AddOrGetCachedComponent<ModifierApplier>();

            foreach (ModifierTarget modifierTarget in targets)
                modifierTarget.Initialize(modifier);

            foreach (ModifierParameterFactory parameterFactory in parameterFactories)
                parameterFactory.Initialize(modifier);
        }

        public override void Execute()
        {
            foreach (object target in targets.SelectMany(x => x.GetTargets()))
            {
                Assert.IsTrue(target is Entity, $"Expecting the target of {nameof(ApplyModifierModifierEffect)} to be of type {nameof(Entity)}");

                bool madeNewApplication = false;
                if ((target as Entity).GetCachedComponent<ModifierHandler>().TryGetUnique(definition, modifierApplier, out ModifierEntity modifier)
                    && (modifier.Behaviours.FirstOrDefault(x => x is StackModifierBehaviour) as StackModifierBehaviour).IsMaxed()
                    && this.modifier.Target.Entity.GetCachedComponent<ModifierHandler>().TryGetModifier(spreadBloodPerk, out ModifierEntity spreadModifier))
                {
                    FactionType faction = modifier.Target.Entity.GetCachedComponent<AgentIdentity>().Faction;

                    float range = spreadModifier[StatisticDefinitionRegistry.Instance.Range];
                    foreach (Target potentialSpreadTarget in Target.All)
                    {
                        if (!potentialSpreadTarget.Entity.TryGetCachedComponent<AgentIdentity>(out AgentIdentity agentIdentity))
                            continue;

                        if (faction != agentIdentity.Faction)
                            continue;

                        if (Mathf.Abs(modifier.Target.Entity.transform.position.x - potentialSpreadTarget.CenterPosition.x) > range)
                            continue;

                        if (potentialSpreadTarget.Entity.GetCachedComponent<ModifierHandler>().TryGetUnique(definition, modifierApplier, out ModifierEntity potentialModifierEntity)
                            && (potentialModifierEntity.Behaviours.FirstOrDefault(x => x is StackModifierBehaviour) as StackModifierBehaviour).IsMaxed())
                        {
                            modifierApplier.Apply(definition, potentialSpreadTarget.Entity.GetCachedComponent<ModifierHandler>(), parameterFactories.Select(x => x.Create(modifier)).ToArray());

                        }
                        else if (!madeNewApplication)
                        {
                            madeNewApplication = true;
                            modifierApplier.Apply(definition, potentialSpreadTarget.Entity.GetCachedComponent<ModifierHandler>(), parameterFactories.Select(x => x.Create(modifier)).ToArray());
                        }
                    }
                }

                modifierApplier.Apply(definition, (target as Entity).GetCachedComponent<ModifierHandler>(), parameterFactories.Select(x => x.Create(modifier)).ToArray());
            }
        }
    }
}
