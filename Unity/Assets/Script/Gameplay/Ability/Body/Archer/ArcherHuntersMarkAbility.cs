namespace Game
{
    public class ArcherHuntersMarkAbility : AnimationBaseCharacterAbility<ArcherHuntersMarkAbilityDefinition>
    {
        [Statistic("damage")] public float Damage => (Caster as Character).AttackPower * definition.DamagePercentage;
        [Statistic("range")] public float Range => (Caster as Character).Reach * definition.ReachPercentage;
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
