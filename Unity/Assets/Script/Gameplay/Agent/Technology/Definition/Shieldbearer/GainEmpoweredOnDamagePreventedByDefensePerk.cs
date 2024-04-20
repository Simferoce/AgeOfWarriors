using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GainEmpoweredOnDamagePreventedByDefensePerk", menuName = "Definition/Technology/Shieldbearer/GainEmpoweredOnDamagePreventedByDefensePerk")]
    public class GainEmpoweredOnDamagePreventedByDefensePerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier>
        {
            private float currentDamagePrevented = 0f;
            private float damageToPreventBeforeGainingBuff;
            private EmpoweredModifierDefinition empoweredModifierDefinition;

            public float CurrentDamagePrevented { get => currentDamagePrevented; set => currentDamagePrevented = value; }

            public Modifier(IModifiable modifiable, ModifierDefinition modifierDefinition, float damageToPreventBeforeGainingBuff, EmpoweredModifierDefinition empoweredModifierDefinition) : base(modifiable, modifierDefinition)
            {
                modifiable.GetCachedComponent<IAttackable>().OnDamageTaken += Modifier_OnDamageTaken;
                this.damageToPreventBeforeGainingBuff = damageToPreventBeforeGainingBuff;
                this.empoweredModifierDefinition = empoweredModifierDefinition;
            }

            private void Modifier_OnDamageTaken(AttackResult attack, IAttackable source)
            {
                currentDamagePrevented += attack.DefenseDamagePrevented;

                if (currentDamagePrevented > damageToPreventBeforeGainingBuff)
                {
                    currentDamagePrevented -= damageToPreventBeforeGainingBuff;

                    Game.Modifier modifier = modifiable.GetModifiers().FirstOrDefault(x => x.Definition == empoweredModifierDefinition);
                    if (modifier != null)
                        modifier.Refresh();
                    else
                        modifiable.AddModifier(new EmpoweredModifierDefinition.Modifier(modifiable, empoweredModifierDefinition));
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

        public override string ParseDescription(object caller, string description)
        {
            description = base.ParseDescription(caller, description);
            if (caller is Modifier modifier)
            {
                description = description.Replace("{val:damage_to_prevent}", (damageToPrevent - modifier.CurrentDamagePrevented).ToString());
            }
            else
            {
                description = description.Replace("{val:damage_to_prevent}", damageToPrevent.ToString());
            }

            description = empoweredModifierDefinition.ParseDescription(caller, description);
            return description;
        }

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, damageToPrevent, empoweredModifierDefinition);
        }
    }
}
