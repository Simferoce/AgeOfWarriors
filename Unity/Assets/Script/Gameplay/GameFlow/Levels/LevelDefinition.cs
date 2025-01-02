using Game.Agent;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "LevelDefinition", menuName = "Definition/LevelDefinition")]
    public class LevelDefinition : Definition
    {
        [SerializeField] private SceneDefinition sceneDefinition;
        [SerializeField] private string title;
        [SerializeField] private AgentLoadout loadout;
        [SerializeReference, SubclassSelector] private List<LevelObjective> objectives;

        public SceneDefinition SceneDefinition { get => sceneDefinition; }
        public string Title { get => title; set => title = value; }
        public AgentLoadout Loadout { get => loadout; set => loadout = value; }
        public List<LevelObjective> Objectives { get => objectives; set => objectives = value; }
    }
}
