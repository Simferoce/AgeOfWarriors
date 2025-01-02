using Game.Character;
using Game.Modifier;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CommanderDefinition", menuName = "Definition/CommanderDefinition")]
    public class CommanderDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject agentPrefab;
        [SerializeField] private ModifierDefinition perk;
        [SerializeField] private CommanderActiveDefinition active;
        [SerializeField] private List<CharacterDefinition> characterDefinitions = new List<CharacterDefinition>();

        public Sprite Icon { get => icon; set => icon = value; }
        public ModifierDefinition Perk { get => perk; set => perk = value; }
        public CommanderActiveDefinition Active { get => active; set => active = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public List<CharacterDefinition> CharacterDefinitions { get => characterDefinitions; set => characterDefinitions = value; }
        public GameObject AgentPrefab { get => agentPrefab; set => agentPrefab = value; }
    }
}
