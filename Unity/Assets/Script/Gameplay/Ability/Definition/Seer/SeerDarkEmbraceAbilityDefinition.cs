using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SeerDarkEmbraceAbilityDefinition", menuName = "Definition/Ability/Seer/SeerDarkEmbraceAbilityDefinition")]
    public class SeerDarkEmbraceAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Damage")]
        [SerializeField, Range(0, 5)] private float damagePercentage;

        [Header("Cooldown")]
        [SerializeField] private float cooldown;

        [Header("Duration")]
        [SerializeField] private float duration;

        public float ReachPercentage => reachPercentage;
        public float DamagePercentage => damagePercentage;
        public float Cooldown => cooldown;
        public float Duration => duration;
    }
}
