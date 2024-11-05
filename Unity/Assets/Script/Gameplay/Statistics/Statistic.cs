using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class Statistic
    {
        [SerializeField] private string name;
        [SerializeField] private StatisticDefinition definition;
        [SerializeReference, SubclassSelector] private Value value;

        public string Name { get => name; set => name = value; }
        public StatisticDefinition Definition { get => definition; set => definition = value; }
        public Value Value { get => value; set => this.value = value; }
    }
}
