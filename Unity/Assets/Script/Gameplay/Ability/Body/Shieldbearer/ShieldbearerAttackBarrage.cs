namespace Game
{
    public class ShieldbearerAttackBarrage : AnimationBaseCharacterAbility<ShieldbearerAttackBarrageAbilityDefinition>
    {
        [Statistic("damage")] public float Damage => Character.AttackPower * definition.DamagePercentage;
        [Statistic("range")] public float Range => Character.Reach * definition.ReachPercentage;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return $"Execute 3 attacks dealing {Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)} damage in a burst on a target at {Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)} meters.";
        }
    }
}
