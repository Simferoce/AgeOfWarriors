using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackSpeedBaseOnStaggerAppliedPerk", menuName = "Definition/Technology/Archer/AttackSpeedBaseOnStaggerAppliedPerk")]
    public class AttackSpeedBaseOnStaggerAppliedPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, AttackSpeedBaseOnStaggerAppliedPerk>
        {
            private int amountOfStaggerApplied = 0;
            private Statistic<float> attackSpeedPercentage;

            public Modifier(AttackSpeedBaseOnStaggerAppliedPerk modifierDefinition) : base(modifierDefinition)
            {
                attackSpeedPercentage = new Statistic<float>(StatisticDefinition.PercentageAttackSpeed);
            }

            public override void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
            {
                base.Initialize(modifiable, source, parameters);
                modifiable.Entity.GetCachedComponent<ModifierApplier>().OnModifierAdded += Modifier_OnModifierAdded;
            }

            public override float? GetStack()
            {
                return amountOfStaggerApplied;
            }

            private void Modifier_OnModifierAdded(Game.Modifier modifier)
            {
                if (modifier is StaggerModifierDefinition.Modifier)
                    amountOfStaggerApplied++;

                attackSpeedPercentage.SetValue(amountOfStaggerApplied * definition.attackSpeedByStaggerApplied);
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.Entity.GetCachedComponent<ModifierApplier>().OnModifierAdded -= Modifier_OnModifierAdded;
            }
        }

        [SerializeField, Range(0, 5)] private float attackSpeedByStaggerApplied;

        public override string ParseDescription()
        {
            return string.Format(Description, attackSpeedByStaggerApplied);
        }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
