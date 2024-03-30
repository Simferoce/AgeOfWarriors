using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AnimationBaseCharacterAbility : CharacterAbility
    {
        [SerializeField] private CharacterAnimatorParameter.Parameter parameter = CharacterAnimatorParameter.Parameter.Ability;
        [Space]
        [SerializeReference, SubclassSelector] private List<AbilityCondition> conditions = new List<AbilityCondition>();
        [SerializeReference, SubclassSelector] private List<AbilityEffect> effects = new List<AbilityEffect>();

        public override bool IsActive { get => IsCasting || IsLingering; }
        public bool IsLingering { get; set; } = false;

        public override void Initialize(Character character)
        {
            base.Initialize(character);

            foreach (AbilityCondition condition in conditions)
                condition.Initialize(character);

            foreach (AbilityEffect effect in effects)
                effect.Initialize(character);
        }

        private void OnCastEnded()
        {
            IsCasting = false;

            if (effects.OfType<ILingeringAbilityEffect>().Count() == 0)
                End();
            else
                IsLingering = true;
        }

        public override void Dispose()
        {
            character.CharacterAnimator.OnAbilityUsed -= OnAnimatorEventAbilityUsed;
            AnimatorEventChannel.Unsubscribe(character.CharacterAnimator.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnCastEnded);
        }

        public override void Update()
        {
            if (IsCasting == true)
                return;

            bool end = true;
            foreach (ILingeringAbilityEffect lingeringAbilityEffect in effects.OfType<ILingeringAbilityEffect>())
            {
                end &= lingeringAbilityEffect.Update(character);
            }

            if (end)
                End();
        }

        public override bool CanUse()
        {
            return conditions.All(x => x.Execute()) && IsCasting == false && IsLingering == false;
        }

        public override void Use()
        {
            character.CharacterAnimator.SetTrigger(parameter);
            character.LastAbilityUsed = Time.time;
            IsCasting = true;

            foreach (AbilityCondition condition in conditions)
                condition.OnAbilityStarted();

            foreach (AbilityEffect effect in effects)
                effect.OnAbilityStarted();

            character.CharacterAnimator.OnAbilityUsed += OnAnimatorEventAbilityUsed;
            AnimatorEventChannel.Subscribe(character.CharacterAnimator.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnCastEnded);
        }

        protected void OnAnimatorEventAbilityUsed()
        {
            foreach (AbilityEffect effect in effects)
                effect.Apply();
        }

        private void End()
        {
            IsLingering = false;

            foreach (AbilityCondition condition in conditions)
                condition.OnAbilityEnded();

            foreach (AbilityEffect effect in effects)
                effect.OnAbilityEnded();

            Dispose();
        }
    }
}
