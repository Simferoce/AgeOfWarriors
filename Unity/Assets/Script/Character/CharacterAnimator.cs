using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        private class Damping
        {
            public float DampTime;
            public float Target;
        }

        public static readonly int SPEED_RATIO = Animator.StringToHash("SpeedRatio");
        public static readonly int ATTACK = Animator.StringToHash("Attack");
        public static readonly int DEAD = Animator.StringToHash("Dead");

        private Animator animator;
        private Dictionary<int, Damping> dampings = new Dictionary<int, Damping>();

        public Animator Animator => animator;
        public event Action OnAbilityUsed;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            foreach (KeyValuePair<int, Damping> damping in dampings)
            {
                animator.SetFloat(damping.Key, damping.Value.Target, damping.Value.DampTime, Time.deltaTime);
            }
        }

        public void SetTrigger(int trigger)
        {
            animator.SetTrigger(trigger);
        }

        public void SetFloat(int trigger, float value)
        {
            animator.SetFloat(trigger, value);
        }

        public void SetFloat(int trigger, float value, float dampTime)
        {
            if (!dampings.ContainsKey(trigger))
            {
                dampings[trigger] = new Damping() { Target = value, DampTime = dampTime };
            }
            else
            {
                dampings[trigger].Target = value;
                dampings[trigger].DampTime = dampTime;
            }
        }

        public void UseAbility()
        {
            OnAbilityUsed?.Invoke();
        }
    }
}