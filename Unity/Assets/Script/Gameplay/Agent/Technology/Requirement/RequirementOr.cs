using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class RequirementOr : Requirement
    {
        [SerializeReference, SubclassSelector] private List<Requirement> requirements;

        public override bool Execute(Agent agent)
        {
            return requirements.Any(x => x.Execute(agent));
        }

        public override string Format(Agent agent)
        {
            return string.Join(" OR ", requirements.Select(x => x.Format(agent)));
        }
    }
}
