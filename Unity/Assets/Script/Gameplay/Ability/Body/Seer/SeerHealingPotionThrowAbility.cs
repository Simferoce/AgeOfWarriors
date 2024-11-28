namespace Game
{
    public class SeerHealingPotionThrowAbility : AnimationBaseCharacterAbility<SeerHealingPotionThrowAbilityDefinition>
    {
        public float Heal => definition.Heal;
        public float Range => Caster.GetCachedComponent<Character>().Reach * definition.ReachPercentage;

        public override float Cooldown => definition.Cooldown;

        public SeerHealingPotionThrowAbility(SeerHealingPotionThrowAbilityDefinition definition, string trigger = "") : base(definition, trigger)
        {
        }

        public override string ParseDescription()
        {
            return string.Format(definition.Description, Heal);
        }
    }
}
