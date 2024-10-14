
using Game;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StatisticRepository
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        statisticDefinitions = null;
    }

    public static StatisticDefinition Damage => GetDefinition("damage");
    public static StatisticDefinition DamagePercentage => GetDefinition("damage_percentage");
    public static StatisticDefinition DamageAgainstWeak => GetDefinition("damage_against_weak");
    public static StatisticDefinition Health => GetDefinition("health");
    public static StatisticDefinition MaxHealth => GetDefinition("health_max");
    public static StatisticDefinition MaxHealthFlat => GetDefinition("health_max_flat");
    public static StatisticDefinition MaxHealthPercentage => GetDefinition("health_max_percentage");
    public static StatisticDefinition Defense => GetDefinition("defense");
    public static StatisticDefinition Cooldown => GetDefinition("cooldown");
    public static StatisticDefinition Duration => GetDefinition("duration");
    public static StatisticDefinition BuffDuration => GetDefinition("buff_duration");
    public static StatisticDefinition Range => GetDefinition("range");
    public static StatisticDefinition Reach => GetDefinition("reach");
    public static StatisticDefinition Leach => GetDefinition("leach");
    public static StatisticDefinition AttackSpeed => GetDefinition("attack_speed");
    public static StatisticDefinition AttackSpeedPercentage => GetDefinition("attack_speed_percentage");
    public static StatisticDefinition AttackPower => GetDefinition("attack_power");
    public static StatisticDefinition AttackPowerPercentage => GetDefinition("attack_power_percentage");
    public static StatisticDefinition Speed => GetDefinition("speed");
    public static StatisticDefinition SpeedPercentage => GetDefinition("speed_percentage");
    public static StatisticDefinition Heal => GetDefinition("heal");
    public static StatisticDefinition DefenseReduction => GetDefinition("defense_reduction");

    private static Dictionary<string, StatisticDefinition> statisticDefinitions = null;

    public static StatisticDefinition GetDefinition(string name)
    {
        if (statisticDefinitions == null)
            statisticDefinitions = Resources.LoadAll<StatisticDefinition>("Definition/Statistic").ToDictionary(x => x.HumanReadableId);

        if (!statisticDefinitions.ContainsKey(name))
            throw new System.Exception($"Unable to find the statistic with the name {name}");

        return statisticDefinitions[name];
    }
}

