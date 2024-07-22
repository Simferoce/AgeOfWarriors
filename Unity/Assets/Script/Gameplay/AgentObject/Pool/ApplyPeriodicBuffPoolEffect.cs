using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    [Serializable]
    public class ApplyPeriodicBuffPoolEffect : PoolEffect
    {
        public abstract class Instancier
        {
            public abstract ModifierDefinition ModifierDefinition { get; }

            public abstract Modifier Instanciate(IModifiable modifiable, IModifierSource modifierSource);
        }

        public Instancier ModifierInstancier { get; set; }

        private List<Modifier> appliedModifiers = new List<Modifier>();

        public override void Apply(Pool pool, ITargeteable targeteable)
        {
            base.Apply(pool, targeteable);

            if (targeteable.Faction != pool.Faction)
                return;

            if (!targeteable.TryGetCachedComponent<IModifiable>(out IModifiable modifiable))
                return;

            Modifier modifier = modifiable.GetModifiers().FirstOrDefault(x => x.Definition == ModifierInstancier.ModifierDefinition);
            if (modifier != null)
                return;

            modifier = ModifierInstancier.Instanciate(modifiable, pool.Owner.GetCachedComponent<IModifierSource>());
            modifiable.AddModifier(modifier);
            appliedModifiers.Add(modifier);
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (Modifier modifier in appliedModifiers)
            {
                //Should remove modifier of object that have been destroyed.
                if (modifier.Modifiable == null)
                    continue;

                modifier.Modifiable.RemoveModifier(modifier);
            }

            appliedModifiers.Clear();
        }
    }
}
