using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CharacterAbility : IDisposable
    {
        [SerializeField] private CharacterAnimatorParameter.Parameter parameter = CharacterAnimatorParameter.Parameter.Ability;
        [Space]
        [SerializeReference, SubclassSelector] private List<AbilityCondition> conditions = new List<AbilityCondition>();
        [SerializeReference, SubclassSelector] private List<AbilityEffect> effects = new List<AbilityEffect>();

        public bool IsActive { get => IsCasting || IsLingering; }
        public bool IsCasting { get; set; }
        public bool IsLingering { get; set; } = false;

        protected Character character;

        public virtual void Initialize(Character character)
        {
            this.character = character;
            character.CharacterAnimator.OnAbilityUsed += OnAnimatorEventAbilityUsed;

            foreach (AbilityCondition condition in conditions)
            {
                condition.Initialize(character);
            }

            AnimatorEventChannel.Subscribe(character.CharacterAnimator.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnCastEnded);
        }

        private void OnCastEnded()
        {
            IsCasting = false;

            if (effects.OfType<ILingeringAbilityEffect>().Count() == 0)
                End();
            else
                IsLingering = true;
        }

        public virtual void Dispose()
        {
            AnimatorEventChannel.Unsubscribe(character.CharacterAnimator.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnCastEnded);
        }

        public virtual void Update()
        {
            bool end = true;
            foreach (ILingeringAbilityEffect lingeringAbilityEffect in effects.OfType<ILingeringAbilityEffect>())
            {
                end &= lingeringAbilityEffect.Update(character);
            }

            if (end)
                End();
        }

        public virtual bool CanUse()
        {
            return conditions.All(x => x.Execute()) && IsCasting == false && IsLingering == false;
        }

        public virtual void Use()
        {
            character.CharacterAnimator.SetTrigger(parameter);
            character.LastAbilityUsed = Time.time;
            IsCasting = true;
        }

        protected void OnAnimatorEventAbilityUsed()
        {
            foreach (AbilityEffect effect in effects)
            {
                effect.Apply(character);
            }
        }

        private void End()
        {
            IsLingering = false;

            foreach (AbilityCondition condition in conditions)
            {
                condition.OnAbilityEnded();
            }

            foreach (AbilityEffect effect in effects)
            {
                effect.OnAbilityEnded();
            }
        }
    }
}
