using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BerserkerLeachingStrikeAbilityDefinition", menuName = "Definition/Ability/Berserker/BerserkerLeachingStrikeAbilityDefinition")]
    public class BerserkerLeachingStrikeAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Damage")]
        [SerializeField, Range(0, 5)] private float damagePercentage;

        [Header("Cooldown")]
        [SerializeField] private float cooldown;

        [Header("Leach")]
        [SerializeField, Range(0, 5)] private float leach;

        [Statistic("damage")] public float Damage(Ability ability) => ability.Character.AttackPower * damagePercentage;
        [Statistic("range")] public float Range(Ability ability) => ability.Character.Reach * reachPercentage;
        [Statistic("cooldown")] public float Cooldown(Ability ability) => cooldown;
        [Statistic("leach")] public float Leach(Ability ability) => leach;
    }
}
