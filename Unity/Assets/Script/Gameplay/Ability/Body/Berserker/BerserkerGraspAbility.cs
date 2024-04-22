namespace Game
{
    public class BerserkerGraspAbility : AnimationBaseCharacterAbility<BerserkerGraspAbilityDefinition>
    {
        [Statistic("range")] public float Range => Character.Reach * definition.ReachPercentage;
        [Statistic("buff_duration")] public float BuffDuration => definition.BuffDuration;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return $"Pull all enemies in {Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)} meters. Staggers the affected enemies for {BuffDuration}. Trigger only if there is at least 2 enemies in range.";
        }
    }
}
