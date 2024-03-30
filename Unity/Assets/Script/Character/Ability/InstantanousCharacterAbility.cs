using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class InstantanousCharacterAbility : CharacterAbility
    {
        [SerializeReference, SubclassSelector] private List<AbilityCondition> conditions = new List<AbilityCondition>();
        [SerializeReference, SubclassSelector] private List<AbilityEffect> effects = new List<AbilityEffect>();

        public override bool CanUse()
        {
            return conditions.All(x => x.Execute());
        }

        public override void Dispose()
        {

        }

        public override void Initialize(Character character)
        {
            base.Initialize(character);

            foreach (AbilityCondition condition in conditions)
                condition.Initialize(character);

            foreach (AbilityEffect effect in effects)
                effect.Initialize(character);
        }

        public override void Update()
        {
        }

        public override void Use()
        {
            foreach (AbilityCondition condition in conditions)
                condition.OnAbilityStarted();

            foreach (AbilityEffect effect in effects)
                effect.OnAbilityStarted();

            foreach (AbilityEffect effect in effects)
                effect.Apply();

            End();
        }

        private void End()
        {
            foreach (AbilityEffect effect in effects)
                effect.OnAbilityEnded();

            foreach (AbilityCondition condition in conditions)
                condition.OnAbilityEnded();
        }
    }
}
