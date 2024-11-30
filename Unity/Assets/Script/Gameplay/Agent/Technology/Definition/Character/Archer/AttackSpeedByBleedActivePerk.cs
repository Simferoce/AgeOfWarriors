using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackSpeedByBleedActivePerk", menuName = "Definition/Technology/Archer/AttackSpeedByBleedActivePerk")]
    public class AttackSpeedByBleedActivePerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, AttackSpeedByBleedActivePerk>
        {
            public override bool Show => amountOfBleedApplied > 0;

            private int amountOfBleedApplied = 0;
            private Statistic<float> attackSpeedPercentage;

            public Modifier(AttackSpeedByBleedActivePerk modifierDefinition) : base(modifierDefinition)
            {
                attackSpeedPercentage = new Statistic<float>(StatisticDefinition.PercentageAttackSpeed);
                StatisticRegistry.Register(attackSpeedPercentage);
            }

            public override float? GetStack()
            {
                return amountOfBleedApplied;
            }

            public override void Update()
            {
                base.Update();

                amountOfBleedApplied = 0;
                foreach (Game.Modifier modifier in modifiable.Entity.GetCachedComponent<ModifierApplier>().AppliedModifiers)
                {
                    if (modifier is not BleedingModifierDefinition.Modifier bleedingModifier)
                        continue;

                    amountOfBleedApplied += (int)modifier.GetStack();
                }

                attackSpeedPercentage.SetValue(amountOfBleedApplied * definition.attackSpeedPerBleedApplied);
            }

            public override void Dispose()
            {
                base.Dispose();
                StatisticRegistry.Unregister(attackSpeedPercentage);
            }
        }

        [SerializeField, Range(0, 5)] private float attackSpeedPerBleedApplied;

        public override string ParseDescription()
        {
            return string.Format(Description, attackSpeedPerBleedApplied);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
