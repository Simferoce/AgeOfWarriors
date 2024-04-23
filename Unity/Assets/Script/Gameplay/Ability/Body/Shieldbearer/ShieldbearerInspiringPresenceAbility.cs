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
            return string.Format(definition.Description,
                Defense,
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}",
                BuffDuration);
        }
    }
}
