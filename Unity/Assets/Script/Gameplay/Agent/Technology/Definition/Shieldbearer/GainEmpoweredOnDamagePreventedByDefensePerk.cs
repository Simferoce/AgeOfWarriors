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

            public Modifier(IModifiable modifiable, GainEmpoweredOnDamagePreventedByDefensePerk modifierDefinition) : base(modifiable, modifierDefinition)
            {
                modifiable.GetCachedComponent<IAttackable>().OnDamageTaken += Modifier_OnDamageTaken;
            }

            private void Modifier_OnDamageTaken(AttackResult attack, IAttackable source)
            {
                currentDamagePrevented += attack.DefenseDamagePrevented;

                if (currentDamagePrevented > definition.damageToPrevent)
                {
                    currentDamagePrevented -= definition.damageToPrevent;

                    Game.Modifier modifier = modifiable.GetModifiers().FirstOrDefault(x => x.Definition == empoweredModifierDefinition);
                    if (modifier != null)
                        modifier.Refresh();
                    else
                        modifiable.AddModifier(new EmpoweredModifierDefinition.Modifier(modifiable, definition.empoweredModifierDefinition));
                }
            }

            public override float? GetStack()
            {
                return currentDamagePrevented;
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.GetCachedComponent<IAttackable>().OnDamageTaken -= Modifier_OnDamageTaken;
            }
        }

        [SerializeField] private float damageToPrevent = 50.0f;
        [SerializeField] private EmpoweredModifierDefinition empoweredModifierDefinition;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this);
        }
    }
}
