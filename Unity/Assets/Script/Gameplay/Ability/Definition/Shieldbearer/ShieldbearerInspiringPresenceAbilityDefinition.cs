using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ShieldbearerInspiringPresenceAbilityDefinition", menuName = "Definition/Ability/Shieldbearer/ShieldbearerInspiringPresenceAbilityDefinition")]
    public partial class ShieldbearerInspiringPresenceAbilityDefinition : AbilityDefinition
    {
        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField] private float defense;
        [SerializeField] private float cooldown;
        [SerializeField] private float buffDuration;

        [Statistic("reach")] public float ReachPercentage => reachPercentage;
        [Statistic("defense")] public float Defense => defense;
        [Statistic("cooldown")] public float Cooldown => cooldown;
        [Statistic("buff_duration")] public float BuffDuration => buffDuration;
    }
}
