namespace Game
{
    public partial class ArcherHuntersMarkAbility : AnimationBaseCharacterAbility<ArcherHuntersMarkAbilityDefinition>
    {
        [Statistic("damage")] public float Damage => Caster.Entity.GetCachedComponent<Character>().AttackPower * definition.DamagePercentage;
        [Statistic("range")] public float Range => Caster.Entity.GetCachedComponent<Character>().Reach * definition.ReachPercentage;
        [Statistic("buff_duration")] public float BuffDuration => definition.BuffDuration;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return string.Format(definition.Description, $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}",
                BuffDuration,
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}");
        }
    }
}
