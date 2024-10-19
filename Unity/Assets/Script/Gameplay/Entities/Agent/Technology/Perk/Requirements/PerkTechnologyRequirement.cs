using Game.UI.Utilities;
using Game.UI.Windows;
using System;
using UnityEngine;

namespace Game.Technology
{
    [Serializable]
    public class PerkTechnologyRequirement : TechnologyRequirement
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
