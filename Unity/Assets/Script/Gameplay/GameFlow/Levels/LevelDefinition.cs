using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "LevelDefinition", menuName = "Definition/LevelDefinition")]
    public class LevelDefinition : Definition
    {
        [SerializeField] private SceneDefinition sceneDefinition;
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private CommanderDefinition commanderDefinition;
        [SerializeReference, SubclassSelector] private List<LevelObjective> objectives;

        public SceneDefinition SceneDefinition { get => sceneDefinition; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public CommanderDefinition CommanderDefinition { get => commanderDefinition; set => commanderDefinition = value; }
        public List<LevelObjective> Objectives { get => objectives; set => objectives = value; }
    }
}
