using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SeerBasicAbilityDefinition", menuName = "Definition/Ability/Seer/SeerBasicAbilityDefinition")]
    public partial class SeerBasicAbilityDefinition : AbilityDefinition
    {
        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField, Range(0, 5)] private float damagePercentage;

        [Statistic("reach")] public float ReachPercentage => reachPercentage;
        [Statistic("damage")] public float DamagePercentage => damagePercentage;
    }
}
