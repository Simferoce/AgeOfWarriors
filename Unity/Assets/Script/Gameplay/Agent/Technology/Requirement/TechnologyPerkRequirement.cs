using System;
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
            return technologyTree.GetStatus(technologyPerkDefinition) is TechnologyPerkStatusUnlocked;
        }

        public override string Format(TechnologyPerkDefinition technologyPerkDefinition, TechnologyTree technologyTree)
        {
            if (technologyTree.GetStatus(technologyPerkDefinition) is TechnologyPerkStatusUnlocked)
                return $"<color=#{ColorUtility.ToHtmlStringRGBA(WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Green))}>{technologyPerkDefinition.Title}</color>";

            return $"<color=#{ColorUtility.ToHtmlStringRGBA(WindowManager.Instance.GetColor(ColorRegistry.Identifiant.Red))}>{technologyPerkDefinition.Title}</color>";
        }
    }
}
