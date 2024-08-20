using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "StaggerProjectileModifierDefinition", menuName = "Definition/Modifier/Projectile/StaggerProjectileModifierDefinition")]
    public class StaggerProjectileModifierDefinition : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, StaggerProjectileModifierDefinition>
        {
            private Projectile projectile;
            private float duration;

            public Modifier(IModifiable modifiable, StaggerProjectileModifierDefinition modifierDefinition, float duration, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                this.duration = duration;

                this.projectile = modifiable.GetCachedComponent<Projectile>();
                projectile.OnImpacted += Projectile_OnImpacted;
            }

            private void Projectile_OnImpacted(List<ITargeteable> targeteables)
            {
                foreach (ITargeteable targeteable in targeteables)
                {
                    if (targeteable.TryGetCachedComponent<IModifiable>(out IModifiable modifiable))
                    {
                        modifiable.AddModifier(new StaggerModifierDefinition.Modifier(modifiable, definition.staggerModifierDefinition, duration, projectile.Caster as Character));
                    }
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                projectile.OnImpacted -= Projectile_OnImpacted;
            }
        }

        [SerializeField] private StaggerModifierDefinition staggerModifierDefinition;
    }
}
