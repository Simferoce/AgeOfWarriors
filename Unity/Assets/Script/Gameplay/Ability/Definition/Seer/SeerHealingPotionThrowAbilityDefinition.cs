using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SeerHealingPotionThrowAbilityDefinition", menuName = "Definition/Ability/Seer/SeerHealingPotionThrowAbilityDefinition")]
    public class SeerHealingPotionThrowAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Heal")]
        [SerializeField] private float heal;

        [Header("Cooldown")]
        [SerializeField] private float cooldown;

        public float ReachPercentage => reachPercentage;
        public float Heal => heal;
        public float Cooldown => cooldown;
    }
}
