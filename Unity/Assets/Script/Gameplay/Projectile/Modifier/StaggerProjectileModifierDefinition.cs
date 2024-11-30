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

            public Modifier(ModifierHandler modifiable, StaggerProjectileModifierDefinition modifierDefinition, float duration, ModifierApplier modifierSource) : base(modifierDefinition)
            {
                this.duration = duration;

                this.projectile = modifiable.Entity.GetCachedComponent<Projectile>();
                projectile.OnImpacted += Projectile_OnImpacted;
            }

            private void Projectile_OnImpacted(List<Target> targeteables)
            {
                foreach (Target targeteable in targeteables)
                {
                    if (targeteable.Entity.TryGetCachedComponent<ModifierHandler>(out ModifierHandler modifiable))
                    {
                        modifiable.AddModifier(new StaggerModifierDefinition.Modifier(modifiable, definition.staggerModifierDefinition, duration, projectile.AgentObject.GetCachedComponent<ModifierApplier>()));
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
