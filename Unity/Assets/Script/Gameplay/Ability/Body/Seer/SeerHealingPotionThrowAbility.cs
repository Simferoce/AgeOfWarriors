namespace Game
{
    public class SeerHealingPotionThrowAbility : AnimationBaseCharacterAbility<SeerHealingPotionThrowAbilityDefinition>
    {
        [Statistic("heal")] public float Heal => definition.Heal;
        [Statistic("range")] public float Range => Character.Reach * definition.ReachPercentage;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return $"Throw a potion that heals an ally on contact by {Heal}.";
        }
    }
}
