namespace Game
{
    public class SeerDarkEmbraceAbility : AnimationBaseCharacterAbility<SeerDarkEmbraceAbilityDefinition>
    {
        public float Damage => Caster.Entity.GetCachedComponent<Character>().AttackPower * definition.DamagePercentage;
        public float Range => Caster.Entity.GetCachedComponent<Character>().Reach * definition.ReachPercentage;
        public float Duration => definition.Duration;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}",
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}");
        }
    }
}
