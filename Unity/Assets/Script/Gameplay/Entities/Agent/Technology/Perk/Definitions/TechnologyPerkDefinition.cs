using Game.Modifier;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Technology
{
    [CreateAssetMenu(fileName = "TechnologyPerkDefinition", menuName = "Definition/Technology/TechnologyPerkDefinition")]
    public class TechnologyPerkDefinition : ModifierDefinition
    {
        [Space]
        [SerializeField] private Sprite technologyTreeIcon;

        [FormerlySerializedAs("requirementsPerk")]
        [SerializeReference, SubclassSelector] private List<TechnologyRequirement> requirements = new List<TechnologyRequirement>();

        public List<TechnologyRequirement> Requirements { get => requirements; set => requirements = value; }
        public Sprite TechnologyTreeIcon { get => technologyTreeIcon; set => technologyTreeIcon = value; }

        public bool IsUnlockable(TechnologyTree technologyTree)
        {
            foreach (TechnologyRequirement requirement in requirements)
            {
                if (!requirement.Execute(this, technologyTree))
                    return false;
            }

            return true;
        }

        public bool HasRequirements()
        {
            return requirements.Count > 0;
        }

        public string FormatRequirements(TechnologyTree technologyTree)
        {
            return string.Join(" AND ", requirements.Select(x => x.Format(this, technologyTree)).Where(x => !string.IsNullOrEmpty(x)));
        }
    }
}