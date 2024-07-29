using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class PerkRequirement : Requirement
    {
        [SerializeField] private TechnologyPerkDefinition technologyPerkDefinition;

        public TechnologyPerkDefinition TechnologyPerkDefinition { get => technologyPerkDefinition; set => technologyPerkDefinition = value; }

        public override bool Execute(Agent agent)
        {
            return agent.Technology.GetStatus(technologyPerkDefinition) is TechnologyHandler.TechnologyPerkStatusUnlocked;
        }

        public override string Format(Agent agent)
        {
            if (agent.Technology.GetStatus(technologyPerkDefinition) is TechnologyHandler.TechnologyPerkStatusUnlocked)
                return $"<color=#{WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Green).ToHexString()}>{technologyPerkDefinition.Title}</color>";

            return $"<color=#{WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Red).ToHexString()}>{technologyPerkDefinition.Title}</color>";
        }
    }
}
