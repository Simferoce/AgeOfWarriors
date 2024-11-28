namespace Game
{
    public class ShieldbearerInspiringPresenceAbility : AnimationBaseCharacterAbility<ShieldbearerInspiringPresenceAbilityDefinition>
    {
        public float Defense => definition.Defense;
        public float Range => Caster.GetCachedComponent<Character>().Reach * definition.ReachPercentage;
        public float BuffDuration => definition.BuffDuration;

        public override float Cooldown => definition.Cooldown;

        public ShieldbearerInspiringPresenceAbility(ShieldbearerInspiringPresenceAbilityDefinition definition, string trigger = "") : base(definition, trigger)
        {
        }

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                Defense,
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}",
                BuffDuration);
        }
    }
}
