using UnityEngine;

namespace Game
{
    public class SeerBasicAbility : AnimationBaseCharacterAbility<SeerBasicAbilityDefinition>
    {
        public float Damage => Caster.GetCachedComponent<Character>().AttackPower * definition.DamagePercentage;
        public float Range => Caster.GetCachedComponent<Character>().Reach * definition.ReachPercentage;

        public override float Cooldown => 0f;

        public SeerBasicAbility(SeerBasicAbilityDefinition definition, string trigger = "") : base(definition, trigger)
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
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.Damage)}",
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}");
        }
    }
}
