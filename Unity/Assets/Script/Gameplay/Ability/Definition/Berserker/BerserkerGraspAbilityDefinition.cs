using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BerserkerGraspAbilityDefinition", menuName = "Definition/Ability/Berserker/BerserkerGraspAbilityDefinition")]
    public partial class BerserkerGraspAbilityDefinition : AbilityDefinition
    {
        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField] private float cooldown;
        [SerializeField] private float buffDuration;

        [Statistic("reach")] public float ReachPercentage => reachPercentage;
        [Statistic("cooldown")] public float Cooldown => cooldown;
        [Statistic("buff_duration")] public float BuffDuration => buffDuration;
    }
}
