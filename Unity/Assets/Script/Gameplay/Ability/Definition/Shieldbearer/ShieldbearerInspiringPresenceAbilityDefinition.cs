using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ShieldbearerInspiringPresenceAbilityDefinition", menuName = "Definition/Ability/Shieldbearer/ShieldbearerInspiringPresenceAbilityDefinition")]
    public class ShieldbearerInspiringPresenceAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Defense")]
        [SerializeField] private float defense;

        [Header("Cooldown")]
        [SerializeField] private float cooldown;

        [Header("Buff Duration")]
        [SerializeField] private float buffDuration;

        [Statistic("defense")] public float Defense(Ability ability) => defense;
        [Statistic("range")] public float Range(Ability ability) => ability.Character.Reach * reachPercentage;
        [Statistic("cooldown")] public float Cooldown(Ability ability) => cooldown;
        [Statistic("buff_duration")] public float BuffDuration(Ability ability) => buffDuration;
    }
}
