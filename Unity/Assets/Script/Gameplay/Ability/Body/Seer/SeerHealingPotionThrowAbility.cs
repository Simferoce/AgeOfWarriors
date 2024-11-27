using System;

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

        public override bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic)
        {
            if (path.StartsWith(StatisticProviderName))
                path = path.Slice(StatisticProviderName.Length + 1);

            if (path.SequenceEqual("heal"))
            {
                float healTemporary = Heal;
                statistic = __refvalue(__makeref(healTemporary), T);
                return true;
            }
            else if (path.SequenceEqual("range"))
            {
                float rangeTemporary = Range;
                statistic = __refvalue(__makeref(rangeTemporary), T);
                return true;
            }
            else
            {
                return base.TryGetStatistic<T>(path, out statistic);
            }
        }

        public override string ParseDescription()
        {
            return string.Format(definition.Description, Heal);
        }
    }
}
