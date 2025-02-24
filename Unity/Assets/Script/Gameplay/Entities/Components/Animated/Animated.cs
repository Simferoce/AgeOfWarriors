﻿using Game.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Components
{
    [RequireComponent(typeof(Animator))]
    public class Animated : MonoBehaviour
    {
        [Serializable]
        private class SerializeAnimationEventDispatcher
        {
            [SerializeField] private string key;
            [SerializeField] private UnityEvent evt;

            public string Key { get => key; set => key = value; }

            public void Dispatch()
            {
                evt?.Invoke();
            }
        }

        private class Damping
        {
            public float DampTime;
            public float Target;
        }

        [SerializeField]
        private List<SerializeAnimationEventDispatcher> serializeAnimationEventDispatchers = new List<SerializeAnimationEventDispatcher>();

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
            Entity.Link(this);
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

        public void DispatchEvent(string key)
        {
            SerializeAnimationEventDispatcher serializeAnimationEventDispatcher = serializeAnimationEventDispatchers.FirstOrDefault(x => x.Key == key);
            if (serializeAnimationEventDispatcher == null)
            {
                Debug.LogError($"Could not find a dispatcher with the key \"{key}\" in \"{this.transform.GetFullPath()}\".", this);
                return;
            }

            serializeAnimationEventDispatcher.Dispatch();
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