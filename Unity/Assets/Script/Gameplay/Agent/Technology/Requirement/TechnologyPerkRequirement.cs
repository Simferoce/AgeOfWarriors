using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Game
{
    [Serializable]
    [MovedFrom(false, "Game", null, "PerkRequirement")]
    public class TechnologyPerkRequirement : TechnologyRequirement
    {
        [SerializeField] private TechnologyPerkDefinition technologyPerkDefinition;

        public TechnologyPerkDefinition TechnologyPerkDefinition { get => technologyPerkDefinition; set => technologyPerkDefinition = value; }

        public override bool Execute(TechnologyPerkDefinition technologyPerkDefinition, TechnologyTree technologyTree)
        {
            return technologyTree.GetStatus(technologyPerkDefinition) is TechnologyHandler.TechnologyPerkStatusUnlocked;
        }

        public override string Format(TechnologyPerkDefinition technologyPerkDefinition, TechnologyTree technologyTree)
        {
            if (technologyTree.GetStatus(technologyPerkDefinition) is TechnologyHandler.TechnologyPerkStatusUnlocked)
                return $"<color=#{WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Green).ToHexString()}>{technologyPerkDefinition.Title}</color>";

            return $"<color=#{WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Red).ToHexString()}>{technologyPerkDefinition.Title}</color>";
        }
    }
}
