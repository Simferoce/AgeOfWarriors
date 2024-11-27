using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ShieldbearerInspiringPresenceAbilityDefinition", menuName = "Definition/Ability/Shieldbearer/ShieldbearerInspiringPresenceAbilityDefinition")]
    public class ShieldbearerInspiringPresenceAbilityDefinition : AbilityDefinition
    {
        [Header("Range")]
        [SerializeField, Range(0, 5)] private float reachPercentage;

        [Header("Defense")]
        [SerializeField] private float defense;

        [Header("Cooldown")]
        [SerializeField] private float cooldown;

        [Header("Buff Duration")]
        [SerializeField] private float buffDuration;

        public float ReachPercentage => reachPercentage;
        public float Defense => defense;
        public float Cooldown => cooldown;
        public float BuffDuration => buffDuration;

        public override Ability GetAbility()
        {
            throw new System.NotImplementedException();
        }
    }
}
