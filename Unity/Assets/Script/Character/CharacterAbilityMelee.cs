using System;

namespace Game
{
    [Serializable]
    public class CharacterAbilityMelee : CharacterAbility
    {
        public override void Initialize(Character character)
        {
            base.Initialize(character);
            character.CharacterAnimator.OnAbilityUsed += OnAnimatorEventAbilityUsed;

            AnimatorEventChannel.Subscribe(character.CharacterAnimator.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnAbilityEnded);
        }

        public override void Dispose()
        {
            base.Dispose();
            AnimatorEventChannel.Unsubscribe(character.CharacterAnimator.Animator, AnimatorEventChannel.Event.OnExit, AnimatorEventChannel.Id.Ability, OnAbilityEnded);
        }

        public void OnAbilityEnded()
        {
            IsCasting = false;
        }

        public override bool CanUse()
        {
            return base.CanUse() && IsCasting == false && character.GetTarget() != null;
        }

        public override void Use()
        {
            base.Use();

            character.CharacterAnimator.SetTrigger(CharacterAnimator.ATTACK);
            IsCasting = true;
        }

        public void OnAnimatorEventAbilityUsed()
        {
            ITargeteable target = character.GetTarget();

            if (target != null)
                target.TakeAttack(character.AttackPower);
        }
    }
}
