using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Modifier
{
    [Serializable]
    public class ApplyModifierModifierEffect : ModifierEffect
    {
        [SerializeField] private ModifierDefinition definition;
        [SerializeReference, SubclassSelector] private List<ModifierTarget> targets;
        [SerializeReference, SubclassSelector] private List<ModifierParameterFactory> parameterFactories;

        private ModifierApplier modifierApplier;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);

            modifierApplier = modifier.AddOrGetCachedComponent<ModifierApplier>();

            foreach (ModifierParameterFactory parameter in parameterFactories)
                parameter.Initialize(modifier);

            foreach (ModifierTarget modifierTarget in targets)
                modifierTarget.Initialize(modifier);
        }

        public override void Execute()
        {
            foreach (object target in targets.SelectMany(x => x.GetTargets()))
            {
                Assert.IsTrue(target is Entity, $"Expecting the target of {nameof(ApplyModifierModifierEffect)} to be of type {nameof(Entity)}");

                modifierApplier.Apply(definition, (target as Entity).GetCachedComponent<ModifierHandler>(), parameterFactories.Select(x => x.Create(modifier)).ToArray());
            }
        }
    }
}
