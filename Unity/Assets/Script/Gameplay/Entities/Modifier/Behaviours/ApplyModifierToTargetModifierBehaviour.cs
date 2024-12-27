using Game.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class ApplyModifierToTargetModifierBehaviour : ModifierBehaviour, IModifierStack
    {
        [SerializeField] private ModifierDefinition definition;
        [SerializeReference, SubclassSelector] private ModifierTarget target;
        [SerializeReference, SubclassSelector] private List<ModifierParameterFactory> parameterFactories;

        private ModifierApplier modifierApplier;

        public float CurrentStack { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);

            modifierApplier = modifier.AddOrGetCachedComponent<ModifierApplier>();
            target.Initialize(modifier);

            foreach (ModifierParameterFactory parameterFactory in parameterFactories)
                parameterFactory.Initialize(modifier);
        }

        public override Result Update()
        {
            List<Entity> targets = target.GetTargets().OfType<Entity>().ToList();
            foreach (Entity target in targets)
            {
                ModifierEntity modifierEntity = modifierApplier.CurrentlyAppliedModifiers.FirstOrDefault(x => x.Definition == definition && x.Target.Entity == target);

                if (target.TryGetCachedComponent<ModifierHandler>(out ModifierHandler modifierHandler))
                    modifierApplier.Apply(definition, modifierHandler, parameterFactories.Select(x => x.Create(modifier)).ToArray());
            }

            for (int i = modifierApplier.CurrentlyAppliedModifiers.Count - 1; i >= 0; i--)
            {
                ModifierEntity modifier = modifierApplier.CurrentlyAppliedModifiers[i];
                if (modifier.Definition == definition && !targets.Contains(modifier.Target.Entity) && !modifier.StatisticRepository.TryGet("duration", out Statistic durationStatistic))
                {
                    modifier.Kill();
                }
            }

            return base.Update();
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (ModifierEntity modifier in modifierApplier.CurrentlyAppliedModifiers)
            {
                if (modifier.Definition == definition && !modifier.StatisticRepository.TryGet("duration", out Statistic durationStatistic))
                {
                    modifier.Kill();
                }
            }
        }
    }
}
