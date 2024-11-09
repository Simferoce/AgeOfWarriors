using Game.Character;
using System;
using UnityEngine;

namespace Game.Statistics
{
    [Serializable]
    public class CharacterStatisticDefinitionData : StatisticDefinitionData
    {
        [SerializeField] private CharacterDefinition characterDefinition;

        public CharacterDefinition CharacterDefinition { get => characterDefinition; set => characterDefinition = value; }
    }
}
