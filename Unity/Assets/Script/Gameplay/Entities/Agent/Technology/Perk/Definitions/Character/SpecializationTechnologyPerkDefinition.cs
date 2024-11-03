using Game.Character;
using UnityEngine;

namespace Game.Technology
{
    [CreateAssetMenu(fileName = "Specialization", menuName = "Definition/Technology/Specialization")]
    public class SpecializationTechnologyPerkDefinition : TechnologyPerkDefinition
    {
        [SerializeField] private CharacterDefinition specializations;

        public CharacterDefinition Specialization { get => specializations; set => specializations = value; }
    }
}
