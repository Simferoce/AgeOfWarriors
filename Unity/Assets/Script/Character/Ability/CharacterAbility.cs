using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class CharacterAbility : IDisposable
    {
        [SerializeField] private CharacterAnimatorParameter.Parameter parameter = CharacterAnimatorParameter.Parameter.Ability;
        [SerializeField] private float cooldown = 0f;

        public bool IsCasting { get; set; }

        protected Character character;
        private float lastUsed = 0f;

        public virtual void Initialize(Character character)
        {
            this.character = character;
            character.CharacterAnimator.OnAbilityUsed += OnAnimatorEventAbilityUsed;

            AnimatorEventChannel.Subscribe(character.CharacterAnimator.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnAbilityEnded);
        }

        private void OnAbilityEnded()
        {
            IsCasting = false;
        }

        public virtual void Dispose()
        {
            AnimatorEventChannel.Unsubscribe(character.CharacterAnimator.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnAbilityEnded);
        }

        public virtual bool CanUse()
        {
            return Time.time - lastUsed > cooldown && Time.time - character.LastAbilityUsed > character.AttackPerSeconds && IsCasting == false && character.GetTarget() != null;
        }

        public virtual void Use()
        {
            character.CharacterAnimator.SetTrigger(parameter);
            character.LastAbilityUsed = Time.time;
            lastUsed = Time.time;
            IsCasting = true;
        }

        protected abstract void OnAnimatorEventAbilityUsed();
    }
}
