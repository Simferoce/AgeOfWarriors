using Game.Agent;
using Game.Technology;
using UnityEngine;

namespace Game.Character
{
    [CreateAssetMenu(fileName = "Character", menuName = "Definition/AgentObject/Character")]
    public class CharacterDefinition : Definition
    {
        [Header("Display")]
        [SerializeField] private Sprite icon;
        [SerializeField] private string title;

        [Header("Base")]
        [SerializeField] private CharacterDefinition baseDefinition;

        [Header("Prefab")]
        [SerializeField] private CharacterEntity prefab;

        [Header("Other")]
        [SerializeField] private TechnologyTreeDefinition technologyTreeDefinition;

        public Sprite Icon { get => icon; }
        public string Title { get => title; set => title = value; }
        public TechnologyTreeDefinition TechnologyTreeDefinition { get => technologyTreeDefinition; set => technologyTreeDefinition = value; }
        public float ProductionDuration => prefab["production_duration"];
        public float Cost => prefab["cost"];

        public bool IsSpecialization(CharacterDefinition agentIdentityDefinition)
        {
            return baseDefinition == agentIdentityDefinition || (baseDefinition?.IsSpecialization(agentIdentityDefinition) ?? false);
        }

        public CharacterEntity Spawn(AgentEntity agent, Vector3 position, int spawnNumber, int direction)
        {
            CharacterEntity character = GameObject.Instantiate(prefab.gameObject, position, Quaternion.identity).GetComponent<CharacterEntity>();
            character.Definition = this;

            AgentIdentity agentIdentity = character.AddOrGetCachedComponent<AgentIdentity>();
            agentIdentity.Set(agent, spawnNumber, direction);

            return character;
        }
    }
}