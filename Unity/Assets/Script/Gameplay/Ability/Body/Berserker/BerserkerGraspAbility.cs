using UnityEngine;

namespace Game
{
    public class BerserkerGraspAbility : AnimationBaseCharacterAbility<BerserkerGraspAbilityDefinition>
    {
        public float Range => Caster.GetCachedComponent<Character>().Reach * definition.ReachPercentage;
        public float BuffDuration => definition.BuffDuration;

        public override float Cooldown => definition.Cooldown;

        public BerserkerGraspAbility(BerserkerGraspAbilityDefinition definition, string trigger = "") : base(definition, trigger)
        {
        }

        public override bool CanUse()
        {
            if (Time.time - Caster.LastAbilityUsed > Caster.AgentObject[StatisticDefinition.AttackSpeed])
                return false;

            return true;
        }

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}",
                BuffDuration);
        }
    }
}
