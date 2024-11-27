using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class ShieldbearerBasicAbility : AnimationBaseCharacterAbility<ShieldbearerBasicAbilityDefinition>
    {
        public float Damage => Caster.GetCachedComponent<Character>().AttackPower * definition.DamagePercentage;
        public float Range => Caster.GetCachedComponent<Character>().Reach * definition.ReachPercentage;

        public override float Cooldown => 0f;

        private List<IAttackable> attackables;

        public ShieldbearerBasicAbility(ShieldbearerBasicAbilityDefinition definition, string trigger) : base(definition, trigger)
        {
        }

        public override bool CanUse()
        {
            if (!base.CanUse())
                return false;

            attackables = TargetUtility.GetTargets((ITargeteable target) => target.Faction != Caster.Faction && Mathf.Abs(this.Caster.CenterPosition.x - target.CenterPosition.x) < Range).OfType<IAttackable>().ToList();
            return attackables.Count > 0;
        }

        public override void InternalApply()
        {
            base.InternalApply();

            foreach (IAttackable attackable in attackables)
            {
                attackable.TakeAttack(AttackUtility.Generate(Caster.AgentObject as IAttackSource, Damage, 0, 0, false, false, true, attackable));
            }
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
            else
            {
                return base.TryGetStatistic<T>(path, out statistic);
            }
        }

        public override string ParseDescription()
        {
            return string.Format(definition.Description,
                $"{Damage}{StatisticFormatter.Percentage(definition.DamagePercentage, StatisticDefinition.AttackPower)}",
                $"{Range}{StatisticFormatter.Percentage(definition.ReachPercentage, StatisticDefinition.Reach)}");
        }
    }
}
