namespace Game
{
    public class BerserkerLeachingStrikeAbility : AnimationBaseCharacterAbility<BerserkerLeachingStrikeAbilityDefinition>
    {
        [Statistic("damage")] public float Damage => Character.AttackPower * definition.DamagePercentage;
        [Statistic("range")] public float Range => Character.Reach * definition.ReachPercentage;
        [Statistic("leach")] public float Leach => definition.Leach;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return $"Deals {Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)} damage to a target at {Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)} meters and heals by {Leach:0.0%} of damage dealt.";
        }
    }
}