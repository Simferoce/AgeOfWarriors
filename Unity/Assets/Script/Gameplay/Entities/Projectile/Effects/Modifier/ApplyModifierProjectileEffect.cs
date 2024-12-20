﻿using Game.Modifier;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Projectile
{
    [Serializable]
    public class ApplyModifierProjectileEffect : ProjectileEffect, IProjectileImpactEffect
    {
        [SerializeField] private ModifierDefinition modifierDefinition;
        [SerializeReference, SubclassSelector] private List<ModifierParameterFactory> parameters;

        public override void Initialize(ProjectileEntity projectile)
        {
            base.Initialize(projectile);

            foreach (ModifierParameterFactory parameterFactory in parameters)
                parameterFactory.Initialize(projectile);
        }

        public void Execute(Entity entity)
        {
            ModifierApplier modifierApplier = projectile.AddOrGetCachedComponent<ModifierApplier>();
            ModifierHandler target = entity.GetCachedComponent<ModifierHandler>();

            modifierApplier.Apply(modifierDefinition, target, parameters.Select(x => x.Create(projectile)).ToArray());
        }
    }
}
