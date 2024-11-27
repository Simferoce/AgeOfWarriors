using System;

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

        public override bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic)
        {
            if (path.StartsWith(StatisticProviderName))
                path = path.Slice(StatisticProviderName.Length + 1);

            if (path.SequenceEqual("defense"))
            {
                float defenseTemporary = Defense;
                statistic = __refvalue(__makeref(defenseTemporary), T);
                return true;
            }
            else if (path.SequenceEqual("range"))
            {
                float rangeTemporary = Range;
                statistic = __refvalue(__makeref(rangeTemporary), T);
                return true;
            }
            else if (path.SequenceEqual("buff_duration"))
            {
                float buffDurationTemporary = BuffDuration;
                statistic = __refvalue(__makeref(buffDurationTemporary), T);
                return true;
            }
            else
            {
                return base.TryGetStatistic<T>(path, out statistic);
            }
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
