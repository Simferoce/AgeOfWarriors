using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        public static readonly int SPEED_RATIO = Animator.StringToHash("SpeedRatio");
        public static readonly int ATTACK = Animator.StringToHash("Attack");
        public static readonly int DEAD = Animator.StringToHash("Dead");

        private Animator animator;

        public Animator Animator => animator;
        public event Action OnAbilityUsed;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetTrigger(int trigger)
        {
            animator.SetTrigger(trigger);
        }

        public void SetFloat(int trigger, float value)
        {
            animator.SetFloat(trigger, value);
        }

        public void UseAbility()
        {
            OnAbilityUsed?.Invoke();
        }
    }
}