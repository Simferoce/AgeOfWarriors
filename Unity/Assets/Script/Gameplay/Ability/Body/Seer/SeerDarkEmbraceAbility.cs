namespace Game
{
    public class SeerDarkEmbraceAbility : AnimationBaseCharacterAbility<SeerDarkEmbraceAbilityDefinition>
    {
        [Statistic("damage")] public float Damage => Character.AttackPower * definition.DamagePercentage;
        [Statistic("range")] public float Range => Character.Reach * definition.ReachPercentage;
        [Statistic("duration")] public float BuffDuration => definition.Duration;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return $"Invoke a medium size dark fissure behind the first enemy in {Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)} meters that deals {Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)} damage to all enemy in range.";
        }
    }
}
