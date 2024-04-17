using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class ChoiceRequirement : Requirement
    {
        [SerializeField] private ChoiceTechnologyDefinition choiceTechnologyPerkDefinition;

        public override bool Execute(Agent agent)
        {
            return choiceTechnologyPerkDefinition.IsUnlocked(agent);
        }
    }
}
