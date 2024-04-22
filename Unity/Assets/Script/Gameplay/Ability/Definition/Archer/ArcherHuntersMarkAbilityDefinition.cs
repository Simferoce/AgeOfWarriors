using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ArcherHuntersMarkAbilityDefinition", menuName = "Definition/Ability/Archer/ArcherHuntersMarkAbilityDefinition")]
    public class ArcherHuntersMarkAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Damage")]
        [SerializeField, Range(0, 5)] private float damagePercentage;

        [Header("Cooldown")]
        [SerializeField] private float cooldown;

        [Header("Buff Duration")]
        [SerializeField] private float buffDuration;

        [Statistic("damage")] public float Damage(Ability ability) => ability.Character.AttackPower * damagePercentage;
        [Statistic("range")] public float Range(Ability ability) => ability.Character.Reach * reachPercentage;
        [Statistic("cooldown")] public float Cooldown(Ability ability) => cooldown;
        [Statistic("buff_duration")] public float BuffDuration(Ability ability) => buffDuration;
    }
}
