using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SeerBasicAbilityDefinition", menuName = "Definition/Ability/Seer/SeerBasicAbilityDefinition")]
    public class SeerBasicAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Damage")]
        [SerializeField, Range(0, 5)] private float damagePercentage;

        public float ReachPercentage => reachPercentage;
        public float DamagePercentage => damagePercentage;
    }
}
