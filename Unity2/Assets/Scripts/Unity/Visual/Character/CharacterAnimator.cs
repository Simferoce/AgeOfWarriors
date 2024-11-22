using UnityEngine;

namespace AgeOfWarriors.Visual
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetFloat(string name, float value)
        {
            animator.SetFloat(name, value);
        }
    }
}
