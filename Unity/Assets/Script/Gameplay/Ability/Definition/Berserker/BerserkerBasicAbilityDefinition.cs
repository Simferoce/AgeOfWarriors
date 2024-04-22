using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BerserkerBasicAbilityDefinition", menuName = "Definition/Ability/Berserker/BerserkerBasicAbilityDefinition")]
    public class BerserkerBasicAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Damage")]
        [SerializeField, Range(0, 5)] private float damagePercentage;

        public float ReachPercentage => reachPercentage;
        public float DamagePercentage => damagePercentage;
    }
}
