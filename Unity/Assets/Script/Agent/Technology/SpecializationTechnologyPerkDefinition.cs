using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Specialization", menuName = "Definition/Technology/Specialization")]
    public class SpecializationTechnologyPerkDefinition : TechnologyPerkDefinition
    {
        [SerializeField] private AgentObjectDefinition specializee;
        [SerializeField] private AgentObjectDefinition specializations;

        public AgentObjectDefinition Specialization { get => specializations; set => specializations = value; }
        public AgentObjectDefinition Specializee { get => specializee; set => specializee = value; }
    }
}
