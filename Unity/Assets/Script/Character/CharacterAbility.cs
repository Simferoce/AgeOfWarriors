using System;

namespace Game
{
    public class CharacterAbility : IDisposable
    {
        public bool IsCasting { get; set; }

        private Character character;

        public CharacterAbility(Character character)
        {
            this.character = character;
            character.CharacterAnimator.OnAbilityUsed += OnAnimatorEventAbilityUsed;

            AnimatorEventChannel.Subscribe(character.CharacterAnimator.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnAbilityEnded);
        }

        public void Dispose()
        {
            AnimatorEventChannel.Unsubscribe(character.CharacterAnimator.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnAbilityEnded);
        }

        public void OnAbilityEnded()
        {
            IsCasting = false;
        }

        public bool CanUse()
        {
            return IsCasting == false && character.GetTarget() != null;
        }

        public void Use()
        {
            character.CharacterAnimator.SetTrigger(CharacterAnimator.ATTACK);
            IsCasting = true;
        }

        public void OnAnimatorEventAbilityUsed()
        {
            ITargeteable target = character.GetTarget();

            if (target != null)
                target.TakeAttack(1);
        }
    }
}
