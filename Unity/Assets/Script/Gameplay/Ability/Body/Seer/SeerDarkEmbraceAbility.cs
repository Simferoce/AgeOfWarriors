namespace Game
{
    public class SeerDarkEmbraceAbility : AnimationBaseCharacterAbility<SeerDarkEmbraceAbilityDefinition>
    {
        [Statistic("damage")] public float Damage => (Caster as Character).AttackPower * definition.DamagePercentage;
        [Statistic("range")] public float Range => (Caster as Character).Reach * definition.ReachPercentage;
        [Statistic("duration")] public float BuffDuration => definition.Duration;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}",
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}");
        }
    }
}
