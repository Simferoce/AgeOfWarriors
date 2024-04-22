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

        [Statistic("heal")] public float Heal(Ability ability) => heal;
        [Statistic("range")] public float Range(Ability ability) => ability.Character.Reach * reachPercentage;
        [Statistic("cooldown")] public float Cooldown(Ability ability) => cooldown;
    }
}
