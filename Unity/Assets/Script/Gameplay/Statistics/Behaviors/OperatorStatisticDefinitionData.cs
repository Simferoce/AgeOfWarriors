using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class OperatorStatisticDefinitionData : StatisticDefinitionData
    {
        [SerializeField] private StatisticOperator statisticOperator;

        public StatisticOperator StatisticOperator { get => statisticOperator; set => statisticOperator = value; }
    }
}
