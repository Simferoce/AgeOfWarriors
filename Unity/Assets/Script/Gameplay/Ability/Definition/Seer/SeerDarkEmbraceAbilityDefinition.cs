using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SeerDarkEmbraceAbilityDefinition", menuName = "Definition/Ability/Seer/SeerDarkEmbraceAbilityDefinition")]
    public class SeerDarkEmbraceAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Damage")]
        [SerializeField, Range(0, 5)] private float damagePercentage;

        [Header("Cooldown")]
        [SerializeField] private float cooldown;

        [Header("Duration")]
        [SerializeField] private float duration;

        [Statistic("damage")] public float Damage(Ability ability) => ability.Character.AttackPower * damagePercentage;
        [Statistic("range")] public float Range(Ability ability) => ability.Character.Reach * reachPercentage;
        [Statistic("cooldown")] public float Cooldown(Ability ability) => cooldown;
        [Statistic("duration")] public float BuffDuration(Ability ability) => duration;
    }
}
