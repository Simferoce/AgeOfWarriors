using Game.Agent;
using Game.Components;
using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]

    public class SpreadBloodModifierBehaviour : ModifierBehaviour
    {
        [SerializeField] private ModifierDefinition definition;
        [SerializeField] private StatisticReference range;

        private ModifierApplier modifierApplier;
        private bool spreading = false;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            range.Initialize(modifier);

            modifierApplier = modifier.Target.Entity.AddOrGetCachedComponent<ModifierApplier>();
            modifierApplier.OnChildModifierApplied += OnChildModifierApplied;
        }

        private void OnChildModifierApplied(ModifierEntity modifier)
        {
            if (spreading)
                return;

            spreading = true;
            Spread(modifier.Target.Entity);
            spreading = false;
        }

        public override void Dispose()
        {
            base.Dispose();
            modifierApplier.OnChildModifierApplied -= OnChildModifierApplied;
        }

        private void Spread(Entity target)
        {
            bool madeNewApplication = false;
            if ((target as Entity).GetCachedComponent<ModifierHandler>().TryGetUnique(definition, modifierApplier, out ModifierEntity modifier)
                && (modifier.Behaviours.FirstOrDefault(x => x is StackModifierBehaviour) as StackModifierBehaviour).IsMaxed())
            {
                FactionType faction = modifier.Target.Entity.GetCachedComponent<AgentIdentity>().Faction;

                foreach (Target potentialSpreadTarget in Target.All)
                {
                    if (!potentialSpreadTarget.Entity.TryGetCachedComponent<AgentIdentity>(out AgentIdentity agentIdentity))
                        continue;

                    if (faction != agentIdentity.Faction)
                        continue;

                    if (Mathf.Abs(modifier.Target.Entity.transform.position.x - potentialSpreadTarget.CenterPosition.x) > range.Get().Get<float>())
                        continue;

                    if (potentialSpreadTarget.Entity.GetCachedComponent<ModifierHandler>().TryGetUnique(definition, modifierApplier, out ModifierEntity potentialModifierEntity)
                        && (potentialModifierEntity.Behaviours.FirstOrDefault(x => x is StackModifierBehaviour) as StackModifierBehaviour).IsMaxed())
                    {
                        modifierApplier.Apply(definition, potentialSpreadTarget.Entity.GetCachedComponent<ModifierHandler>(), potentialModifierEntity.Parameters.Select(x => x.Clone()).ToArray());
                    }

                    else if (!madeNewApplication)
                    {
                        madeNewApplication = true;
                        modifierApplier.Apply(definition, potentialSpreadTarget.Entity.GetCachedComponent<ModifierHandler>(), modifier.Parameters.Select(x => x.Clone()).ToArray());
                    }
                }
            }
        }
    }
}
