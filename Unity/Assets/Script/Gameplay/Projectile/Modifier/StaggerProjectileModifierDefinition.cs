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

            public Modifier(ModifierHandler modifiable, StaggerProjectileModifierDefinition modifierDefinition, float duration, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                this.duration = duration;

                this.projectile = modifiable.Entity.GetCachedComponent<Projectile>();
                projectile.OnImpacted += Projectile_OnImpacted;
            }

            private void Projectile_OnImpacted(List<ITargeteable> targeteables)
            {
                foreach (ITargeteable targeteable in targeteables)
                {
                    if ((targeteable as Entity).TryGetCachedComponent<ModifierHandler>(out ModifierHandler modifiable))
                    {
                        modifiable.AddModifier(new StaggerModifierDefinition.Modifier(modifiable, definition.staggerModifierDefinition, duration, projectile.AgentObject as Character));
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
