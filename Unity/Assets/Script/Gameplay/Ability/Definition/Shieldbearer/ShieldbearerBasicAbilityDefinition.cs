using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ShieldbearerBasicAbilityDefinition", menuName = "Definition/Ability/Shieldbearer/ShieldbearerBasicAbilityDefinition")]
    public class ShieldbearerBasicAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Damage")]
        [SerializeField, Range(0, 5)] private float damagePercentage;

        public float ReachPercentage => reachPercentage;
        public float DamagePercentage => damagePercentage;
    }
}
