using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class BaseStatisticDefinitionData : StatisticDefinitionData
    {
        [SerializeField] private StatisticDefinition definition;

        public StatisticDefinition Definition { get => definition; set => definition = value; }
    }
}
