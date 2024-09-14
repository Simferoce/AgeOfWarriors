namespace Game
{
    public class ArcherHuntersMarkAbility : AnimationBaseCharacterAbility<ArcherHuntersMarkAbilityDefinition>
    {
        public float Damage => Caster.Entity.GetCachedComponent<Character>().AttackPower * definition.DamagePercentage;
        public float Range => Caster.Entity.GetCachedComponent<Character>().Reach * definition.ReachPercentage;
        public float BuffDuration => definition.BuffDuration;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return string.Format(definition.Description, $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}",
                BuffDuration,
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}");
        }
    }
}
