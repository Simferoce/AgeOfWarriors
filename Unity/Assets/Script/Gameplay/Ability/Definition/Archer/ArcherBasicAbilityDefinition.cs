using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ArcherBasicAbilityDefinition", menuName = "Definition/Ability/Archer/ArcherBasicAbilityDefinition")]
    public class ArcherBasicAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Damage")]
        [SerializeField, Range(0, 5)] private float damagePercentage;

        public float ReachPercentage => reachPercentage;
        public float DamagePercentage => damagePercentage;

        public override Ability GetAbility()
        {
            throw new System.NotImplementedException();
        }
    }
}
