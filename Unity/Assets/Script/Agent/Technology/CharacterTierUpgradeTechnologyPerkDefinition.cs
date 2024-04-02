using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CharacterTierUpgrade", menuName = "Definition/Technology/CharacterTierUpgrade")]
    public class CharacterTierUpgradeTechnologyPerkDefinition : TechnologyPerkDefinition
    {
        [SerializeField] private AgentObjectDefinition toReplace;
        [SerializeField] private AgentObjectDefinition into;

        public AgentObjectDefinition ToReplace { get => toReplace; set => toReplace = value; }
        public AgentObjectDefinition Into { get => into; set => into = value; }
    }
}
