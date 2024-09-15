using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ShieldbearerAttackBarrageAbilityDefinition", menuName = "Definition/Ability/Shieldbearer/ShieldbearerAttackBarrageAbilityDefinition")]
    public partial class ShieldbearerAttackBarrageAbilityDefinition : AbilityDefinition
    {
        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField, Range(0, 5)] private float damagePercentage;
        [SerializeField] private float cooldown;

        [Statistic("reach")] public float ReachPercentage => reachPercentage;
        [Statistic("damage")] public float DamagePercentage => damagePercentage;
        [Statistic("cooldown")] public float Cooldown => cooldown;
    }
}