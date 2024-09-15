namespace Game
{
    public partial class SeerDarkEmbraceAbility : AnimationBaseCharacterAbility<SeerDarkEmbraceAbilityDefinition>
    {
        [Statistic("damage")] public float Damage => Caster.Entity.GetCachedComponent<Character>().AttackPower * definition.DamagePercentage;
        [Statistic("range")] public float Range => Caster.Entity.GetCachedComponent<Character>().Reach * definition.ReachPercentage;
        [Statistic("duration")] public float Duration => definition.Duration;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}",
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}");
        }
    }
}
