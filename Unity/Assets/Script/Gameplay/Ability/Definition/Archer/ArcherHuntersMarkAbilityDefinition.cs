using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ArcherHuntersMarkAbilityDefinition", menuName = "Definition/Ability/Archer/ArcherHuntersMarkAbilityDefinition")]
    public class ArcherHuntersMarkAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Damage")]
        [SerializeField, Range(0, 5)] private float damagePercentage;

        [Header("Cooldown")]
        [SerializeField] private float cooldown;

        [Header("Buff Duration")]
        [SerializeField] private float buffDuration;

        public float ReachPercentage { get => reachPercentage; set => reachPercentage = value; }
        public float DamagePercentage { get => damagePercentage; set => damagePercentage = value; }
        public float Cooldown { get => cooldown; set => cooldown = value; }
        public float BuffDuration { get => buffDuration; set => buffDuration = value; }
    }
}
