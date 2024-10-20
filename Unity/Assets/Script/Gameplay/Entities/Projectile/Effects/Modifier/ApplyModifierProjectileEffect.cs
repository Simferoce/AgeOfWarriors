using Game.Modifier;
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

        public void Execute(Entity entity)
        {
            ModifierApplier modifierApplier = projectile.AddOrGetCachedComponent<ModifierApplier>();
            ModifierHandler target = entity.GetCachedComponent<ModifierHandler>();

            if (target.TryGetModifier(modifierDefinition, out ModifierEntity modifier))
                modifier.Refresh();
            else
                modifierApplier.Apply(modifierDefinition.Instantiate(), target, parameters.Select(x => x.Create(projectile)).ToArray());
        }
    }
}
