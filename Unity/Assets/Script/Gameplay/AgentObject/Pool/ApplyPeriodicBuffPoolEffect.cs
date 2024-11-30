using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public class ApplyPeriodicBuffPoolEffect : PoolEffect
    {
        private List<Modifier> appliedModifiers = new List<Modifier>();

        public override void Apply(Pool pool, Target targeteable)
        {
            base.Apply(pool, targeteable);

            throw new System.NotImplementedException();
            //if (!ModifierInstancier.Applicable(pool, targeteable))
            //    return;

            //if (!targeteable.Entity.TryGetCachedComponent<ModifierHandler>(out ModifierHandler modifiable))
            //    return;

            //Modifier modifier = modifiable.GetModifiers().FirstOrDefault(x => x.Definition == ModifierInstancier.ModifierDefinition);
            //if (modifier != null)
            //    return;

            //modifier = ModifierInstancier.Instanciate(modifiable, pool.GetCachedComponent<ModifierApplier>());
            //modifiable.AddModifier(modifier);
            //appliedModifiers.Add(modifier);
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
