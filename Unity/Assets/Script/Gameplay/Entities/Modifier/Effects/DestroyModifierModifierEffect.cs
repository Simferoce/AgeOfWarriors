using System;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class DestroyModifierModifierEffect : ModifierEffect
    {
        [SerializeReference, SubclassSelector] private ModifierTarget modifierTarget;
        [SerializeField] private ModifierDefinition definition;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            modifierTarget.Initialize(modifier);
        }

        public override void Execute()
        {
            if ((modifierTarget.GetTargets()[0] as Entity).GetCachedComponent<ModifierHandler>().TryGetUnique(definition, null, out ModifierEntity modifier))
            {
                modifier.Kill();
            }
        }
    }
}
