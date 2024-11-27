using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ArcherPiercingShotAbilityDefinition", menuName = "Definition/Ability/Archer/ArcherPiercingShotAbilityDefinition")]
    public class ArcherPiercingShotAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Damage")]
        [SerializeField, Range(0, 5)] private float damagePercentage;

        [Header("Cooldown")]
        [SerializeField] private float cooldown;

        [Header("Armor Penetration")]
        [SerializeField] private float armorPenetration;

        public float ReachPercentage => reachPercentage;
        public float DamagePercentage => damagePercentage;
        public float Cooldown => cooldown;
        public float ArmorPenetration => armorPenetration;

        public override Ability GetAbility()
        {
            throw new System.NotImplementedException();
        }
    }
}
