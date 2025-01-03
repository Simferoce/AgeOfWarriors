﻿using Game.Modifier;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Ability
{
    [Serializable]
    public class ApplyModifierAbilityEffect : AbilityEffect
    {
        [SerializeField] private ModifierDefinition modifierDefinition;
        [SerializeField] private bool targetSelf;
        [SerializeReference, SubclassSelector] private List<ModifierParameterFactory> parameters;

        public override void Initialize(AbilityEntity ability)
        {
            base.Initialize(ability);
            foreach (ModifierParameterFactory parameterFactory in parameters)
                parameterFactory.Initialize(ability);
        }

        public override void Apply()
        {
            List<ModifierHandler> targets = Ability.Targets.Select(x => x.Entity.GetCachedComponent<ModifierHandler>()).ToList();
            Assert.IsFalse(targets.Any(x => x == null), "Expecting all target to be modifiable.");

            ModifierApplier modifierApplier = Ability.GetCachedComponent<ModifierApplier>();

            if (targetSelf)
            {
                modifierApplier.Apply(modifierDefinition, Ability.Caster.Entity.GetCachedComponent<ModifierHandler>(), parameters.Select(x => x.Create(Ability)).ToArray());
            }
            else
            {
                foreach (ModifierHandler target in targets)
                {
                    modifierApplier.Apply(modifierDefinition, target, parameters.Select(x => x.Create(Ability)).ToArray());
                }
            }
        }
    }
}
