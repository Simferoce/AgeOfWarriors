using System;

namespace Game
{
    public class BerserkerLeachingStrikeAbility : AnimationBaseCharacterAbility<BerserkerLeachingStrikeAbilityDefinition>
    {
        public float Damage => Caster.GetCachedComponent<Character>().AttackPower * definition.DamagePercentage;
        public float Range => Caster.GetCachedComponent<Character>().Reach * definition.ReachPercentage;
        public float Leach => definition.Leach;

        public override float Cooldown => definition.Cooldown;

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
            else if (path.SequenceEqual("leach"))
            {
                float leachTemporary = Leach;
                statistic = __refvalue(__makeref(leachTemporary), T);
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
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}",
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}",
                Leach);
        }
    }
}