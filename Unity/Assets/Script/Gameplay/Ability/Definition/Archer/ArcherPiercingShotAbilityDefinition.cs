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

        [Statistic("damage")] public float Damage(Ability ability) => ability.Character.AttackPower * damagePercentage;
        [Statistic("range")] public float Range(Ability ability) => ability.Character.Reach * reachPercentage;
        [Statistic("cooldown")] public float Cooldown(Ability ability) => cooldown;
        [Statistic("armor_penetration")] public float ArmorPenetration(Ability ability) => armorPenetration;
    }
}
