using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Components
{
    [RequireComponent(typeof(Animator))]
    public class Animated : MonoBehaviour
    {
        private class Damping
        {
            public float DampTime;
            public float Target;
        }

        public event Action OnAbilityUsed;

        public Animator Animator => animator;
        public Entity Entity { get; set; }

        private Animator animator;
        private Dictionary<string, Damping> dampings = new Dictionary<string, Damping>();
        private string currentState;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            Entity = GetComponentInParent<Entity>();
        }

        private void Update()
        {
            foreach (KeyValuePair<string, Damping> damping in dampings)
            {
                animator.SetFloat(damping.Key, damping.Value.Target, damping.Value.DampTime, Time.deltaTime);
            }
        }

        public void Play(string animationState)
        {
            currentState = animationState;
            animator.CrossFade(animationState, 0.1f);
        }

        public string GetCurrent()
        {
            return currentState;
        }

        public float GetCurrentAnimationClipLength()
        {
            return animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        }

        public void SetSpeed(float speed)
        {
            animator.speed = speed;
        }

        public void SetFloat(string key, float value)
        {
            animator.SetFloat(key, value);
        }

        public void SetFloat(string key, float value, float dampTime)
        {
            if (!dampings.ContainsKey(key))
            {
                dampings[key] = new Damping() { Target = value, DampTime = dampTime };
            }
            else
            {
                dampings[key].Target = value;
                dampings[key].DampTime = dampTime;
            }
        }

        public void UseAbility()
        {
            OnAbilityUsed?.Invoke();
        }
    }
}