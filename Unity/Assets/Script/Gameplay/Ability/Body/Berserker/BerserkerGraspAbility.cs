namespace Game
{
    public partial class BerserkerGraspAbility : AnimationBaseCharacterAbility<BerserkerGraspAbilityDefinition>
    {
        [Statistic("range")] public float Range => Caster.Entity.GetCachedComponent<Character>().Reach * definition.ReachPercentage;
        [Statistic("buff_duration")] public float BuffDuration => definition.BuffDuration;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}",
                BuffDuration);
        }
    }
}
