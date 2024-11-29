using UnityEngine;

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

        public override bool CanUse()
        {
            if (Time.time - Caster.LastAbilityUsed > Caster.AgentObject[StatisticDefinition.AttackSpeed])
                return false;

            return true;
        }

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}",
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}");
        }
    }
}
