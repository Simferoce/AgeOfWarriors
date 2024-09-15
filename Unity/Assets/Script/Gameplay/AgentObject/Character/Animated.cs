using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class Animated : MonoBehaviour, IComponent
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

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            foreach (KeyValuePair<string, Damping> damping in dampings)
            {
                animator.SetFloat(damping.Key, damping.Value.Target, damping.Value.DampTime, Time.deltaTime);
            }
        }

        public void ClearTrigger(string trigger)
        {
            animator.ResetTrigger(trigger);
        }

        public void SetTrigger(string trigger)
        {
            animator.SetTrigger(trigger);
        }

        public void SetBool(string parameter, bool value)
        {
            animator.SetBool(parameter, value);
        }

        public void SetFloat(string trigger, float value)
        {
            animator.SetFloat(trigger, value);
        }

        public void SetFloat(string trigger, float value, float dampTime)
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