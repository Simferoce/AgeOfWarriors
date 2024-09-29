using System;

namespace Game
{
    [Serializable]
    public class ApplyPeriodicBuffPoolEffect : PoolEffect
    {
        public abstract class Instancier
        {
            public abstract ModifierDefinition ModifierDefinition { get; }

            public abstract bool Applicable(Pool pool, Target targeteable);

            public abstract Modifier Instanciate(ModifierHandler target);
        }

        public Instancier ModifierInstancier { get; set; }
        public override void Apply(Pool pool, Target targeteable)
        {
            base.Apply(pool, targeteable);

            if (!ModifierInstancier.Applicable(pool, targeteable))
                return;

            if (!targeteable.Entity.TryGetCachedComponent<ModifierHandler>(out ModifierHandler modifiable))
                return;

            //Modifier modifier = modifiable.GetModifiers().FirstOrDefault(x => x.Definition == ModifierInstancier.ModifierDefinition);
            //if (modifier != null)
            //    return;

            throw new Exception();
            //modifier = ModifierInstancier.Instanciate(modifiable, pool.AddOrGetCachedComponent<Ownership>().Owner.Entity.GetCachedComponent<IModifierSource>());
            //modifiable.AddModifier(modifier);
            //appliedModifiers.Add(modifier);
        }

        public override void Dispose()
        {
            base.Dispose();

            //foreach (Modifier modifier in appliedModifiers)
            //{
            //    //Should remove modifier of object that have been destroyed.
            //    if (modifier.Modifiable == null)
            //        continue;

            //    modifier.Modifiable.RemoveModifier(modifier);
            //}
        }
    }
}
