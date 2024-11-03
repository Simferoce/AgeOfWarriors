using Game.Character;
using Game.Modifier;
using UnityEngine;

namespace Game.Statistics
{
    [CreateAssetMenu(fileName = "CharacterStatisticDefinition", menuName = "Definition/Statistic/CharacterStatisticDefinition")]
    public class CharacterStatisticDefinition : StatisticDefinition
    {
        [SerializeField] private CharacterDefinition characterDefinition;
        [SerializeField] private StatisticDefinition outputDefinition;
        [SerializeField] private ModifierDefinition modifierDefinition;

        public CharacterDefinition CharacterDefinition { get => characterDefinition; set => characterDefinition = value; }
        public StatisticDefinition OutputDefinition { get => outputDefinition; set => outputDefinition = value; }
        public ModifierDefinition ModifierDefinition { get => modifierDefinition; set => modifierDefinition = value; }
    }
}
