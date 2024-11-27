using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class AttackFactory : MonoBehaviour
    {
        public delegate void OnAttackDealtDelegate(AttackResult attack);
        public event OnAttackDealtDelegate OnAttackDealt;

        public Entity Entity { get; private set; }

        private void Awake()
        {
            Entity = GetComponent<Entity>();
        }

        public Attack Generate(float damage, float armorPenetration, float leach, bool ranged, bool overtime, bool reflectable, Attackable attackable)
        {
            bool empowered = false;

            List<Modifier> modifiers = Entity.GetCachedComponent<ModifierHandler>().GetModifiers();

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

            if (attackable.Entity.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is DamageDealtReductionModifierDefinition.Modifier))
                damage += Entity.GetCachedComponent<ModifierHandler>().GetModifiers().Sum(x => x.DamageDealtAgainstWeak ?? 0);

            return new Attack(this, damage, armorPenetration, leach, ranged, empowered, reflectable, overtime);
        }

        public void NotifyAttackLanded(AttackResult attackResult)
        {
            OnAttackDealt?.Invoke(attackResult);
        }
    }
}
