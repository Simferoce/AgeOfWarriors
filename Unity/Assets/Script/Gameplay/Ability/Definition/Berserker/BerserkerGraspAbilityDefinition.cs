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

        [Statistic("range")] public float Range(Ability ability) => ability.Character.Reach * reachPercentage;
        [Statistic("cooldown")] public float Cooldown(Ability ability) => cooldown;
        [Statistic("buff_duration")] public float BuffDuration(Ability ability) => buffDuration;
    }
}
