using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AttackSpeedBaseOnStaggerAppliedPerk", menuName = "Definition/Technology/Archer/AttackSpeedBaseOnStaggerAppliedPerk")]
    public class AttackSpeedBaseOnStaggerAppliedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, AttackSpeedBaseOnStaggerAppliedPerk>
        {
            private StatisticModifiable<float> attackPowerPercentage = new StatisticModifiable<float>(definition: StatisticRepository.DamagePercentage);

            private int amountOfStaggerApplied = 0;

            public Modifier(ModifierHandler modifiable, AttackSpeedBaseOnStaggerAppliedPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                attackPowerPercentage.Initialize(this);
                modifiable.Entity.GetCachedComponent<IModifierSource>().OnModifierAdded += Modifier_OnModifierAdded;
            }

            public override float? GetStack()
            {
                return amountOfStaggerApplied;
            }

            private void Modifier_OnModifierAdded(Game.Modifier modifier)
            {
                if (modifier is StaggerModifierDefinition.Modifier)
                {
                    amountOfStaggerApplied++;
                    attackPowerPercentage.Modify(amountOfStaggerApplied * definition.attackSpeedByStaggerApplied);
                }
            }

            public override void Dispose()
            {
                base.Dispose();
                modifiable.Entity.GetCachedComponent<IModifierSource>().OnModifierAdded -= Modifier_OnModifierAdded;
            }
        }

        [SerializeField] public StatisticSerialize<float> attackSpeedByStaggerApplied = new StatisticSerialize<float>("attack_speed_per_stagger", null, 1f);

        public override IEnumerable<Statistic> GetStatistic()
        {
            yield return attackSpeedByStaggerApplied;

            foreach (Statistic statistic in base.GetStatistic())
                yield return statistic;
        }

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
