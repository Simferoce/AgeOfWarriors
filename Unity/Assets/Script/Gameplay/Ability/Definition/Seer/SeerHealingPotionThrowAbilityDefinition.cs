using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SeerHealingPotionThrowAbilityDefinition", menuName = "Definition/Ability/Seer/SeerHealingPotionThrowAbilityDefinition")]
    public partial class SeerHealingPotionThrowAbilityDefinition : AbilityDefinition
    {
        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField] private float heal;
        [SerializeField] private float cooldown;

        [Statistic("reach")] public float ReachPercentage => reachPercentage;
        [Statistic("heal")] public float Heal => heal;
        [Statistic("cooldown")] public float Cooldown => cooldown;
    }
}
