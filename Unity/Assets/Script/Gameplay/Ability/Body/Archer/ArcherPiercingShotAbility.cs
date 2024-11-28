namespace Game
{
    public class ArcherPiercingShotAbility : AnimationBaseCharacterAbility<ArcherPiercingShotAbilityDefinition>
    {
        public float Damage => Caster.GetCachedComponent<Character>().AttackPower * definition.DamagePercentage;
        public float Range => Caster.GetCachedComponent<Character>().Reach * definition.ReachPercentage;
        public float ArmorPenetration => definition.ArmorPenetration;

        public override float Cooldown => definition.Cooldown;

        public ArcherPiercingShotAbility(ArcherPiercingShotAbilityDefinition definition, string trigger = "") : base(definition, trigger)
        {
        }

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}",
                ArmorPenetration);
        }
    }
}
