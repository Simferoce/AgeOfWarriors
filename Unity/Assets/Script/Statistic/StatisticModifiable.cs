using System;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticModifiableFloat : Statistic<float>, IStatisticFloat
    {
        private enum ModifiableStatistic
        {
            MaxHealth,
            Defense,
            AttackPower,
            Speed,
        }

        [SerializeField] private StatisticDefinition definition;
        [SerializeField] private StatisticReference<IModifiable> modifiableReference;
        [SerializeField] private StatisticReference<Statistic<float>> baseStatistic;
        [SerializeField] private ModifiableStatistic flat;
        [SerializeField] private ModifiableStatistic percentage;

        public override StatisticDefinition GetDefinition(StatisticContext context)
        {
            return definition;
        }

        public override float GetValue(StatisticContext context)
        {
            IModifiable modifiable = modifiableReference.Get(context);
            float flatValue = 0f;
            switch (flat)
            {
                case ModifiableStatistic.MaxHealth:
                    flatValue = modifiable.GetModifiers().Select(x => x.MaxHealth).Where(x => x.HasValue).Sum(x => x.Value);
                    break;
                case ModifiableStatistic.Defense:
                    flatValue = modifiable.GetModifiers().Select(x => x.Defense).Where(x => x.HasValue).Sum(x => x.Value);
                    break;
                case ModifiableStatistic.AttackPower:
                    flatValue = modifiable.GetModifiers().Select(x => x.AttackPower).Where(x => x.HasValue).Sum(x => x.Value);
                    break;
                case ModifiableStatistic.Speed:
                    flatValue = modifiable.GetModifiers().Select(x => x.SpeedPercentage).Where(x => x.HasValue).Sum(x => x.Value);
                    break;
            }


            float percentageValue = 0f;
            switch (percentage)
            {
                case ModifiableStatistic.MaxHealth:
                    percentageValue = modifiable.GetModifiers().Select(x => x.MaxHealth).Where(x => x.HasValue).Sum(x => x.Value);
                    break;
                case ModifiableStatistic.Defense:
                    percentageValue = modifiable.GetModifiers().Select(x => x.Defense).Where(x => x.HasValue).Sum(x => x.Value);
                    break;
                case ModifiableStatistic.AttackPower:
                    percentageValue = modifiable.GetModifiers().Select(x => x.AttackPower).Where(x => x.HasValue).Sum(x => x.Value);
                    break;
                case ModifiableStatistic.Speed:
                    percentageValue = modifiable.GetModifiers().Select(x => x.SpeedPercentage).Where(x => x.HasValue).Sum(x => x.Value);
                    break;
            }

            return (baseStatistic.Get(context).GetValue(context) + flatValue) * percentageValue;
        }
    }
}
