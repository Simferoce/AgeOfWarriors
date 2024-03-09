using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        public static readonly int SPEED_RATIO = Animator.StringToHash("SpeedRatio");
        public static readonly int ATTACK = Animator.StringToHash("Attack");
        public static readonly int DEAD = Animator.StringToHash("Dead");
        public static readonly int LAYER_UPPER_BODY = 1;

        private Animator animator;

        public Animator Animator => animator;
        public event Action Attacked;

        private Dictionary<int, float> layerWeights = new Dictionary<int, float>();

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            foreach (KeyValuePair<int, float> layerWeigth in layerWeights)
            {
                float value = Mathf.Lerp(animator.GetLayerWeight(layerWeigth.Key), layerWeigth.Value, 0.1f);
                animator.SetLayerWeight(layerWeigth.Key, value);
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

        public void SetLayerWeight(int index, float weight)
        {
            if (!layerWeights.ContainsKey(index))
                layerWeights.Add(index, weight);
            else
                layerWeights[index] = weight;
        }

        public void Attack()
        {
            Attacked?.Invoke();
        }
    }
}