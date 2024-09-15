using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "SeerDarkEmbraceAbilityDefinition", menuName = "Definition/Ability/Seer/SeerDarkEmbraceAbilityDefinition")]
    public class SeerDarkEmbraceAbilityDefinition : AbilityDefinition
    {
        [SerializeField, Range(0, 5)] private float reachPercentage;
        [SerializeField, Range(0, 5)] private float damagePercentage;
        [SerializeField] private float cooldown;
        [SerializeField] private float duration;

        [Statistic("reach")] public float ReachPercentage => reachPercentage;
        [Statistic("damage")] public float DamagePercentage => damagePercentage;
        [Statistic("cooldown")] public float Cooldown => cooldown;
        [Statistic("duration")] public float Duration => duration;
    }
}
