namespace Game
{
    public class ShieldbearerInspiringPresenceAbility : AnimationBaseCharacterAbility<ShieldbearerInspiringPresenceAbilityDefinition>
    {
        [Statistic("defense")] public float Defense => definition.Defense;
        [Statistic("range")] public float Range => Character.Reach * definition.ReachPercentage;
        [Statistic("buff_duration")] public float BuffDuration => definition.BuffDuration;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return $"Increase defense by {Defense} of any ally in {Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)} meters for {BuffDuration} seconds.";
        }
    }
}
