
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

    public static string Damage = "damage";
    public static string DamagePercentage = "damage_percentage";
    public static string DamageAgainstWeak = "damage_against_weak";
    public static string Health = "health";
    public static string MaxHealth = "health_max";
    public static string MaxHealthFlat = "health_max_flat";
    public static string MaxHealthPercentage = "health_max_percentage";
    public static string Defense = "defense";
    public static string Cooldown = "cooldown";
    public static string Duration = "duration";
    public static string BuffDuration = "buff_duration";
    public static string Range = "range";
    public static string Reach = "reach";
    public static string Leach = "leach";
    public static string AttackSpeed = "attack_speed";
    public static string AttackSpeedPercentage = "attack_speed_percentage";
    public static string AttackPower = "attack_power";
    public static string AttackPowerPercentage = "attack_power_percentage";
    public static string Speed = "speed";
    public static string SpeedPercentage = "speed_percentage";
    public static string Heal = "heal";
    public static string DefenseReduction = "defense_reduction";

    public static List<StatisticDefinition> statisticDefinitions = null;

    public static StatisticDefinition GetDefinition(string name)
    {
        if (statisticDefinitions == null)
            statisticDefinitions = Resources.LoadAll<StatisticDefinition>("Definition/Statistic").ToList();

        foreach (StatisticDefinition statisticDefinition in statisticDefinitions)
        {
            if (statisticDefinition.HumanReadableId == name)
                return statisticDefinition;
        }

        throw new System.Exception($"Unable to find the statistic with the name {name}");
    }
}

