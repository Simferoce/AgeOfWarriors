using Game.Character;
using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class CharacterStatisticBehavior : StatisticBehavior
    {
        [SerializeField] private CharacterDefinition characterDefinition;
        [SerializeField] private StatisticDefinition outputDefinition;

        public CharacterDefinition CharacterDefinition { get => characterDefinition; set => characterDefinition = value; }
        public StatisticDefinition OutputDefinition { get => outputDefinition; set => outputDefinition = value; }
    }
}
