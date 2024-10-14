using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    [Serializable]
    public class ModifierEffectApplyModifier : ModifierEffect
    {
        [SerializeField] private ModifierDefinition definition;
        [SerializeReference, SubclassSelector] private List<ModifierTarget> targets;
        [SerializeReference, SubclassSelector] private List<ModifierParameterFactory> parameterFactories;

        private ModifierApplier modifierApplier;

        public override void Initialize(Modifier modifier)
        {
            base.Initialize(modifier);

            modifierApplier = modifier.AddOrGetCachedComponent<ModifierApplier>();

            foreach (ModifierTarget modifierTarget in targets)
                modifierTarget.Initialize(modifier);
        }

        public override void Execute()
        {
            foreach (object target in targets.SelectMany(x => x.GetTargets()))
            {
                Assert.IsTrue(target is Entity, $"Expecting the target of {nameof(ModifierEffectApplyModifier)} to be of type {nameof(Entity)}");

                Modifier modifier = definition.Instantiate();
                modifierApplier.Apply(modifier, (target as Entity).GetCachedComponent<ModifierHandler>(), parameterFactories.Select(x => x.Create(modifier)).ToArray());
            }
        }
    }
}
