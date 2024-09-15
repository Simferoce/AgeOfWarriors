using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BerserkerLeachingStrikeAbilityDefinition", menuName = "Definition/Ability/Berserker/BerserkerLeachingStrikeAbilityDefinition")]
    public partial class BerserkerLeachingStrikeAbilityDefinition : AbilityDefinition
    {
        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField, Range(0, 5)] private float damagePercentage;
        [SerializeField] private float cooldown;
        [SerializeField, Range(0, 5)] private float leach;

        [Statistic("reach")] public float ReachPercentage { get => reachPercentage; set => reachPercentage = value; }
        [Statistic("damage")] public float DamagePercentage { get => damagePercentage; set => damagePercentage = value; }
        [Statistic("cooldown")] public float Cooldown { get => cooldown; set => cooldown = value; }
        [Statistic("leach")] public float Leach { get => leach; set => leach = value; }
    }
}
