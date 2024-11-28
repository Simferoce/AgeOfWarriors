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

        private List<Attackable> attackables;

        public ShieldbearerBasicAbility(ShieldbearerBasicAbilityDefinition definition, string trigger) : base(definition, trigger)
        {
        }

        public override bool CanUse()
        {
            if (!base.CanUse())
                return false;

            attackables = TargetUtility.GetTargets((ITargeteable target) => target.Faction != Caster.Faction && Mathf.Abs(this.Caster.CenterPosition.x - target.CenterPosition.x) < Range).Select(x => (x as Entity).GetCachedComponent<Attackable>()).Where(x => x != null).ToList();
            return attackables.Count > 0;
        }

        public override void InternalApply()
        {
            base.InternalApply();

            foreach (Attackable attackable in attackables)
            {
                attackable.TakeAttack(Caster.Entity.GetCachedComponent<AttackFactory>().Generate(Damage, 0, 0, false, false, true, attackable));
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
