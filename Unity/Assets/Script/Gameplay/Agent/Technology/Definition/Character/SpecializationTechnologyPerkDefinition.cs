using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Specialization", menuName = "Definition/Technology/Specialization")]
    public class SpecializationTechnologyPerkDefinition : TechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, SpecializationTechnologyPerkDefinition>
        {
            public Modifier(SpecializationTechnologyPerkDefinition modifierDefinition) : base(modifierDefinition)
            {
            }
        }

        [SerializeField] private AgentObjectDefinition specializations;

        public AgentObjectDefinition Specialization { get => specializations; set => specializations = value; }

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }
    }
}
