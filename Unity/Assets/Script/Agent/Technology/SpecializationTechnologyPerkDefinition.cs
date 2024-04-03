using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Specialization", menuName = "Definition/Technology/Specialization")]
    public class SpecializationTechnologyPerkDefinition : TechnologyPerkDefinition
    {
        [SerializeField] private AgentObjectDefinition specializations;

        public AgentObjectDefinition Specialization { get => specializations; set => specializations = value; }
    }
}
