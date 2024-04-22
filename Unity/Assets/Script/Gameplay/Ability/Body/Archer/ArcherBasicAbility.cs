namespace Game
{
    public class ArcherBasicAbility : AnimationBaseCharacterAbility<ArcherBasicAbilityDefinition>
    {
        [Statistic("damage")] public float Damage => Character.AttackPower * definition.DamagePercentage;
        [Statistic("range")] public float Range => Character.Reach * definition.ReachPercentage;

        public override float Cooldown => 0f;

        public override string ParseDescription()
        {
            return $"Throw an arrow that deals {Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)} damage in {Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)} meters.";
        }
    }
}
