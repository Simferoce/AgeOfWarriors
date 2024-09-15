using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BerserkerBasicAbilityDefinition", menuName = "Definition/Ability/Berserker/BerserkerBasicAbilityDefinition")]
    public partial class BerserkerBasicAbilityDefinition : AbilityDefinition
    {
        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField, Range(0, 5)] private float damagePercentage;

        [Statistic("reach")] public float ReachPercentage => reachPercentage;
        [Statistic("damage")] public float DamagePercentage => damagePercentage;
    }
}
