using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ShieldbearerBasicAbilityDefinition", menuName = "Definition/Ability/Shieldbearer/ShieldbearerBasicAbilityDefinition")]
    public partial class ShieldbearerBasicAbilityDefinition : AbilityDefinition
    {
        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField, Range(0, 5)] private float damagePercentage;

        [Statistic("reach")] public float ReachPercentage => reachPercentage;
        [Statistic("damage")] public float DamagePercentage => damagePercentage;
    }
}
