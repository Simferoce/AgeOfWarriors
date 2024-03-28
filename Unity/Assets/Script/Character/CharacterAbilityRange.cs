using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CharacterAbilityRange : CharacterAbility
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float angle = 30f;
        [SerializeField] private Transform origin;

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
            {
                GameObject gameObject = GameObject.Instantiate(projectilePrefab, origin.transform.position, Quaternion.identity);
                Projectile projectile = gameObject.GetComponent<Projectile>();

                projectile.Initialize(character.Agent, character.AttackPower, angle, target.Position);
            }
        }
    }
}
