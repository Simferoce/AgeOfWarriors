using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BerserkerGraspAbilityDefinition", menuName = "Definition/Ability/Berserker/BerserkerGraspAbilityDefinition")]
    public class BerserkerGraspAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Cooldown")]
        [SerializeField] private float cooldown;

        [Header("Buff Duration")]
        [SerializeField] private float buffDuration;

        public float ReachPercentage => reachPercentage;
        public float Cooldown => cooldown;
        public float BuffDuration => buffDuration;
    }
}
