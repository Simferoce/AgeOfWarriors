﻿namespace Game
{
    public class ArcherPiercingShotAbility : AnimationBaseCharacterAbility<ArcherPiercingShotAbilityDefinition>
    {
        [Statistic("damage")] public float Damage => Character.AttackPower * definition.DamagePercentage;
        [Statistic("range")] public float Range => Character.Reach * definition.ReachPercentage;
        [Statistic("armor_penetration")] public float ArmorPenetration => definition.ArmorPenetration;

        public override float Cooldown => definition.Cooldown;

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}",
                ArmorPenetration);
        }
    }
}
