namespace Game
{
    public class SeerBasicAbility : AnimationBaseCharacterAbility<SeerBasicAbilityDefinition>
    {
        [Statistic("damage")] public float Damage => (Caster as Character).AttackPower * definition.DamagePercentage;
        [Statistic("range")] public float Range => (Caster as Character).Reach * definition.ReachPercentage;

        public override float Cooldown => 0f;

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.Damage)}",
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}");
        }
    }
}
