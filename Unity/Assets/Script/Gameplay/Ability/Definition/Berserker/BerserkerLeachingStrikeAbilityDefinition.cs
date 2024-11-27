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

        public float ReachPercentage { get => reachPercentage; set => reachPercentage = value; }
        public float DamagePercentage { get => damagePercentage; set => damagePercentage = value; }
        public float Cooldown { get => cooldown; set => cooldown = value; }
        public float Leach { get => leach; set => leach = value; }

        public override Ability GetAbility()
        {
            throw new System.NotImplementedException();
        }
    }
}
