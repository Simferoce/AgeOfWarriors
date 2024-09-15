using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ArcherPiercingShotAbilityDefinition", menuName = "Definition/Ability/Archer/ArcherPiercingShotAbilityDefinition")]
    public partial class ArcherPiercingShotAbilityDefinition : AbilityDefinition
    {
        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField, Range(0, 5)] private float damagePercentage;
        [SerializeField] private float cooldown;
        [SerializeField] private float armorPenetration;

        [Statistic("reach")] public float ReachPercentage => reachPercentage;
        [Statistic("damage")] public float DamagePercentage => damagePercentage;
        [Statistic("cooldown")] public float Cooldown => cooldown;
        [Statistic("armor_penetration")] public float ArmorPenetration => armorPenetration;
    }
}
