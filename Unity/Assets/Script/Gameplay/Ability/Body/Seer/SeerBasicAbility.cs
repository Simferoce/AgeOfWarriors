namespace Game
{
    public class SeerBasicAbility : AnimationBaseCharacterAbility<SeerBasicAbilityDefinition>
    {
        [Statistic("damage")] public float Damage => Character.AttackPower * definition.DamagePercentage;
        [Statistic("range")] public float Range => Character.Reach * definition.ReachPercentage;

        public override float Cooldown => 0f;

        public override string ParseDescription()
        {
            return $"Throw a dark sphere that deals {Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.Damage)} damage to an enemy in {Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)} meters.";
        }
    }
}
