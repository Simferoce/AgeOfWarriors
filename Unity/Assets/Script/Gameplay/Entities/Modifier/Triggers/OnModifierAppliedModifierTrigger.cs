using Game.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class OnModifierAppliedModifierTrigger : ModifierTrigger, IModifierTargetProvider
    {
        [SerializeField] private ModifierDefinition modifierDefinition;

        private CharacterEntity character;
        private ModifierApplier applier;
        private Entity modifierTarget;

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
            {
                modifierTarget = modifier.Target.Entity;
                Trigger();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            applier.OnChildModifierApplied -= OnChildModifierApplied;
        }

        public List<object> GetTargets()
        {
            return new List<object>() { modifierTarget };
        }
    }
}
