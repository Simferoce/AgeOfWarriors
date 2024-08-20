namespace Game
{
    public class BerserkerLeachingStrikeAbility : AnimationBaseCharacterAbility<BerserkerLeachingStrikeAbilityDefinition>
    {
        [Statistic("damage")] public float Damage => (Caster as Character).AttackPower * definition.DamagePercentage;
        [Statistic("range")] public float Range => (Caster as Character).Reach * definition.ReachPercentage;
        [Statistic("leach")] public float Leach => definition.Leach;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}",
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}",
                Leach);
        }
    }
}