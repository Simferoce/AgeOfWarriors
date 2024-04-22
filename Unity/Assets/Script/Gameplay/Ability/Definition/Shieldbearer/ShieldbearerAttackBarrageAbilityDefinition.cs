using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ShieldbearerAttackBarrageAbilityDefinition", menuName = "Definition/Ability/Shieldbearer/ShieldbearerAttackBarrageAbilityDefinition")]
    public class ShieldbearerAttackBarrageAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Damage")]
        [SerializeField, Range(0, 5)] private float damagePercentage;

        [Header("Cooldown")]
        [SerializeField] private float cooldown;

        [Statistic("damage")] public float Damage(Ability ability) => ability.Character.AttackPower * damagePercentage;
        [Statistic("range")] public float Range(Ability ability) => ability.Character.Reach * reachPercentage;
        [Statistic("cooldown")] public float Cooldown(Ability ability) => cooldown;
    }
}