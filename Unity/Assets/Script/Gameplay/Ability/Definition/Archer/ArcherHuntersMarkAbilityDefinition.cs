using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ArcherHuntersMarkAbilityDefinition", menuName = "Definition/Ability/Archer/ArcherHuntersMarkAbilityDefinition")]
    public partial class ArcherHuntersMarkAbilityDefinition : AbilityDefinition
    {
        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField, Range(0, 5)] private float damagePercentage;
        [SerializeField] private float cooldown;
        [SerializeField] private float buffDuration;

        [Statistic("reach")] public float ReachPercentage { get => reachPercentage; set => reachPercentage = value; }
        [Statistic("damage")] public float DamagePercentage { get => damagePercentage; set => damagePercentage = value; }
        [Statistic("cooldown")] public float Cooldown { get => cooldown; set => cooldown = value; }
        [Statistic("buff_duration")] public float BuffDuration { get => buffDuration; set => buffDuration = value; }
    }
}
