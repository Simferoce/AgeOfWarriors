using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainEmpoweredOnDamagePreventedByDefensePerk", menuName = "Definition/Technology/Shieldbearer/GainEmpoweredOnDamagePreventedByDefensePerk")]
    public class GainEmpoweredOnDamagePreventedByDefensePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, GainEmpoweredOnDamagePreventedByDefensePerk>
        {
            private float currentDamagePrevented = 0f;
            private EmpoweredModifierDefinition empoweredModifierDefinition;

            public float CurrentDamagePrevented { get => currentDamagePrevented; set => currentDamagePrevented = value; }

            public Modifier(ModifierHandler modifiable, GainEmpoweredOnDamagePreventedByDefensePerk modifierDefinition, IModifierSource modifierSource) : base(modifiable, modifierDefinition, modifierSource)
            {
                modifiable.Entity.GetCachedComponent<Attackable>().OnDamageTaken += Modifier_OnDamageTaken;
            }

            private void Modifier_OnDamageTaken(AttackResult attack, Attackable source)
            {
                currentDamagePrevented += attack.DefenseDamagePrevented;

                if (currentDamagePrevented > definition.damageToPrevent)
                {
                    currentDamagePrevented -= definition.damageToPrevent;

                    Game.Modifier modifier = modifiable.GetModifiers().FirstOrDefault(x => x.Definition == empoweredModifierDefinition);
                    if (modifier != null)
                        modifier.Refresh();
                    else
                        modifiable.AddModifier(new EmpoweredModifierDefinition.Modifier(
                            modifiable,
                            definition.empoweredModifierDefinition,
                            Source));
                }
            }

            public override float? GetStack()
            {
                return currentDamagePrevented;
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.Entity.GetCachedComponent<Attackable>().OnDamageTaken -= Modifier_OnDamageTaken;
            }
        }

        [SerializeField] private float damageToPrevent = 50.0f;
        [SerializeField] private EmpoweredModifierDefinition empoweredModifierDefinition;

        public override string ParseDescription()
        {
            return string.Format(Description, damageToPrevent, empoweredModifierDefinition.PercentageDamageIncrease);
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}