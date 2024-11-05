using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class ModifyStatisticBehavior : StatisticBehavior
    {
        [SerializeField] private StatisticDefinition definition;
        [SerializeField] private StatisticOperator statisticOperator;

        public StatisticDefinition Definition { get => definition; set => definition = value; }
        public StatisticOperator StatisticOperator { get => statisticOperator; set => statisticOperator = value; }
    }
}
