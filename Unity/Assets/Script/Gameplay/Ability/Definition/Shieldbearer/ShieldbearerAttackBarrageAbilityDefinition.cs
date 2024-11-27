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

        public float ReachPercentage => reachPercentage;
        public float DamagePercentage => damagePercentage;
        public float Cooldown => cooldown;

        public override Ability GetAbility()
        {
            throw new System.NotImplementedException();
        }
    }
}