namespace Game
{
    public partial class SeerHealingPotionThrowAbility : AnimationBaseCharacterAbility<SeerHealingPotionThrowAbilityDefinition>
    {
        [Statistic("heal")] public float Heal => definition.Heal;
        [Statistic("range")] public float Range => Caster.Entity.GetCachedComponent<Character>().Reach * definition.ReachPercentage;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return string.Format(definition.Description, Heal);
        }
    }
}
