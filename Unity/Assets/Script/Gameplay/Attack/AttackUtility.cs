using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public static class AttackUtility
    {
        public static Attack Generate(IAttackSource attacker, float damage, float armorPenetration, float leach, bool ranged, bool overtime, bool reflectable, IAttackable target, params IAttackSource[] source)
        {
            bool empowered = false;

            List<Modifier> modifiers = (attacker as IComponent).GetCachedComponent<ModifierHandler>().GetModifiers();

            EmpoweredModifierDefinition.Modifier empowerment = modifiers.FirstOrDefault(x => x is EmpoweredModifierDefinition.Modifier) as EmpoweredModifierDefinition.Modifier;
            if (empowerment != null)
            {
                damage *= 1 + empowerment.PercentageDamageIncrease;
                empowerment.Consume();

                empowered = true;
            }

            if (modifiers.Count > 0)
            {
                float damageDealtReduction = modifiers.Max(x => x.DamageDealtReduction ?? 0);
                damage *= (1 - damageDealtReduction);
            }

            if (target.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is DamageDealtReductionModifierDefinition.Modifier))
                damage += (attacker as IComponent).GetCachedComponent<ModifierHandler>().GetModifiers().Sum(x => x.DamageDealtAgainstWeak ?? 0);

            return new Attack(new AttackSource(attacker).Add(source), damage, armorPenetration, leach, ranged, empowered, reflectable, overtime);
        }
    }
}