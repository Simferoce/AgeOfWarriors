using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainEmpoweredOnDamagePreventedByDefensePerk", menuName = "Definition/Technology/Shieldbearer/GainEmpoweredOnDamagePreventedByDefensePerk")]
    public class GainEmpoweredOnDamagePreventedByDefensePerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, GainEmpoweredOnDamagePreventedByDefensePerk>
        {
            private float currentDamagePrevented = 0f;
            private EmpoweredModifierDefinition empoweredModifierDefinition;

            public float CurrentDamagePrevented { get => currentDamagePrevented; set => currentDamagePrevented = value; }

            public Modifier(GainEmpoweredOnDamagePreventedByDefensePerk modifierDefinition) : base(modifierDefinition)
            {
            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                modifiable.Entity.GetCachedComponent<Attackable>().OnAttackTaken += Modifier_OnDamageTaken;
            }

            private void Modifier_OnDamageTaken(AttackResult attack)
            {
                currentDamagePrevented += attack.DefenseDamagePrevented;

                if (currentDamagePrevented > definition.damageToPrevent)
                {
                    currentDamagePrevented -= definition.damageToPrevent;

                    Game.Modifier modifier = modifiable.GetModifiers().FirstOrDefault(x => x.Definition == empoweredModifierDefinition);
                    if (modifier != null)
                        modifier.Refresh();
                    else
                        Source.Apply(modifiable, new EmpoweredModifierDefinition.Modifier(
                            definition.empoweredModifierDefinition));
                }
            }

            public override float? GetStack()
            {
                return currentDamagePrevented;
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.Entity.GetCachedComponent<Attackable>().OnAttackTaken -= Modifier_OnDamageTaken;
            }
        }

        [SerializeField] private float damageToPrevent = 50.0f;
        [SerializeField] private EmpoweredModifierDefinition empoweredModifierDefinition;

        public override string ParseDescription()
        {
            return string.Format(Description, damageToPrevent, empoweredModifierDefinition.PercentageDamageIncrease);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}