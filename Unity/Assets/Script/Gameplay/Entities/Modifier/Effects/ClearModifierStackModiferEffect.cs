using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class ClearModifierStackModiferEffect : ModifierEffect
    {
        [SerializeReference, SubclassSelector] private ModifierTarget target;
        [SerializeField] private ModifierDefinition definition;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            target.Initialize(modifier);
        }

        public override void Execute()
        {
            ModifierApplier modifierApplier = this.modifier.AddOrGetCachedComponent<ModifierApplier>();
            foreach (Entity entity in target.GetTargets().OfType<Entity>())
            {
                ModifierHandler modifierHandler = entity.GetCachedComponent<ModifierHandler>();
                if (modifierHandler.TryGetUnique(definition, modifierApplier, out ModifierEntity modifierEntity))
                {
                    StackModifierBehaviour stackModifierBehaviour = modifierEntity.Behaviours.FirstOrDefault(x => x is StackModifierBehaviour) as StackModifierBehaviour;
                    stackModifierBehaviour?.Clear();
                }
            }
        }
    }
}
