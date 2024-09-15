namespace Game
{
    public partial class BerserkerBasicAbility : AnimationBaseCharacterAbility<BerserkerBasicAbilityDefinition>
    {
        [Statistic("damage")] public float Damage => Caster.Entity.GetCachedComponent<Character>().AttackPower * definition.DamagePercentage;
        [Statistic("range")] public float Range => Caster.Entity.GetCachedComponent<Character>().Reach * definition.ReachPercentage;

        public override float Cooldown => 0f;

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}",
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}");
        }
    }
}
