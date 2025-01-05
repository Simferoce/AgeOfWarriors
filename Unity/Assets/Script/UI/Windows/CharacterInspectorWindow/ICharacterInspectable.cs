using Game.Statistics;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Windows
{
    public interface ICharacterInspectable
    {
        public Sprite GetIcon();
        public string GetTitle();
        public Statistic GetStatistic(StatisticDefinition definition);
        public List<IAbilityInspectable> GetAbilityInspectables();
        public List<IModifierInspectable> GetModifierInspectables();
    }
}
