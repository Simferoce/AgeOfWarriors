using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ChoiceTechnologyDefinition", menuName = "Definition/Technology/ChoiceTechnologyDefinition")]
    public class ChoiceTechnologyDefinition : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [Space]
        [SerializeField] private List<TechnologyPerkDefinition> choices;
        [Space]
        [SerializeReference, SubclassSelector] private List<Requirement> requirementsPerk = new List<Requirement>();

        public List<TechnologyPerkDefinition> Choices { get => choices; set => choices = value; }
        public List<Requirement> RequirementsPerk { get => requirementsPerk; set => requirementsPerk = value; }

        public virtual bool IsUnlocked(Agent agent)
        {
            return choices.Any(x => agent.Technology.PerksUnlocked.Contains(x));
        }

        public virtual bool IsUnlockable(Agent agent)
        {
            return RequirementsPerk.Any(x => x.Execute(agent));
        }
    }
}
