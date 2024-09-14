namespace Game
{
    public class ShieldbearerAttackBarrage : AnimationBaseCharacterAbility<ShieldbearerAttackBarrageAbilityDefinition>
    {
        public float Damage => Caster.Entity.GetCachedComponent<Character>().AttackPower * definition.DamagePercentage;
        public float Range => Caster.Entity.GetCachedComponent<Character>().Reach * definition.ReachPercentage;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}",
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}");
        }
    }
}
