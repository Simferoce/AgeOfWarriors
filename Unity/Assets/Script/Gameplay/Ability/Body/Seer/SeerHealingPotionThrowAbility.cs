namespace Game
{
    public class SeerHealingPotionThrowAbility : AnimationBaseCharacterAbility<SeerHealingPotionThrowAbilityDefinition>
    {
        public float Heal => definition.Heal;
        public float Range => Caster.Entity.GetCachedComponent<Character>().Reach * definition.ReachPercentage;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return string.Format(definition.Description, Heal);
        }
    }
}
