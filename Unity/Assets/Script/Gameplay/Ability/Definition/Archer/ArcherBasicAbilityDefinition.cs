using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ArcherBasicAbilityDefinition", menuName = "Definition/Ability/Archer/ArcherBasicAbilityDefinition")]
    public partial class ArcherBasicAbilityDefinition : AbilityDefinition
    {
        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField, Range(0, 5)] private float damagePercentage;

        [Statistic("reach")] public float ReachPercentage => reachPercentage;
        [Statistic("damage")] public float DamagePercentage => damagePercentage;
    }
}
