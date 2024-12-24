using Game.Character;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class OnModifierAppliedModifierTrigger : ModifierTrigger
    {
        [SerializeField] private ModifierDefinition modifierDefinition;

        private CharacterEntity character;
        private ModifierApplier applier;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            character = modifier.GetHierarchy().FirstOrDefault(x => x is CharacterEntity) as CharacterEntity;
            applier = character.GetCachedComponent<ModifierApplier>();
            applier.OnChildModifierApplied += OnChildModifierApplied;
        }

        private void OnChildModifierApplied(ModifierEntity modifier)
        {
            if (modifier.Definition == modifierDefinition)
                Trigger();
        }

        public override void Dispose()
        {
            base.Dispose();
            applier.OnChildModifierApplied -= OnChildModifierApplied;
        }
    }
}
