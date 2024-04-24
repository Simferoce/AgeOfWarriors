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

        private Animator animator;
        private Dictionary<int, Damping> dampings = new Dictionary<int, Damping>();
        private Character character;

        public Animator Animator => animator;
        public event Action OnAbilityUsed;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            character = GetComponentInParent<Character>();
        }

        private void Update()
        {
            foreach (KeyValuePair<int, Damping> damping in dampings)
            {
                animator.SetFloat(damping.Key, damping.Value.Target, damping.Value.DampTime, Time.deltaTime);
            }
        }

        public void ClearTrigger(CharacterAnimatorParameter.Parameter trigger)
        {
            animator.ResetTrigger(trigger.Convert());
            //Debug.Log($"{character.name}:{character.GetInstanceID()} Clear Trigger for {trigger}");
        }

        public void SetTrigger(CharacterAnimatorParameter.Parameter trigger)
        {
            animator.SetTrigger(trigger.Convert());
            //Debug.Log($"{character.name}:{character.GetInstanceID()} Set Trigger for {trigger}");
        }

        public void SetBool(CharacterAnimatorParameter.Parameter parameter, bool value)
        {
            animator.SetBool(parameter.Convert(), value);
        }

        public void SetFloat(CharacterAnimatorParameter.Parameter trigger, float value)
        {
            animator.SetFloat(trigger.Convert(), value);
        }

        public void SetFloat(CharacterAnimatorParameter.Parameter trigger, float value, float dampTime)
        {
            if (!dampings.ContainsKey(trigger.Convert()))
            {
                dampings[trigger.Convert()] = new Damping() { Target = value, DampTime = dampTime };
            }
            else
            {
                dampings[trigger.Convert()].Target = value;
                dampings[trigger.Convert()].DampTime = dampTime;
            }
        }

        public void UseAbility()
        {
            OnAbilityUsed?.Invoke();
        }

    }
}