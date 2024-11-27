using System;

namespace Game
{
    public class SeerDarkEmbraceAbility : AnimationBaseCharacterAbility<SeerDarkEmbraceAbilityDefinition>
    {
        public float Damage => Caster.GetCachedComponent<Character>().AttackPower * definition.DamagePercentage;
        public float Range => Caster.GetCachedComponent<Character>().Reach * definition.ReachPercentage;
        public float Duration => definition.Duration;

        public override float Cooldown => definition.Cooldown;

        public SeerDarkEmbraceAbility(SeerDarkEmbraceAbilityDefinition definition, string trigger = "") : base(definition, trigger)
        {
        }

        public override bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic)
        {
            if (path.StartsWith(StatisticProviderName))
                path = path.Slice(StatisticProviderName.Length + 1);

            if (path.SequenceEqual("damage"))
            {
                float damageTemporary = Damage;
                statistic = __refvalue(__makeref(damageTemporary), T);
                return true;
            }
            else if (path.SequenceEqual("range"))
            {
                float rangeTemporary = Range;
                statistic = __refvalue(__makeref(rangeTemporary), T);
                return true;
            }
            else if (path.SequenceEqual("duration"))
            {
                float durationTemporary = Duration;
                statistic = __refvalue(__makeref(durationTemporary), T);
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
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}",
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}");
        }
    }
}
