using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public abstract class ModifierDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;
        [SerializeReference, SerializeReferenceDropdown] private List<Statistic> statistics;

        public Sprite Icon { get => icon; }
        public string Title { get => title; }
        public string Description { get => description; }

        public bool TryGetValue<T>(object context, StatisticDefinition definition, out T value)
        {
            Statistic<T> statistic = statistics.FirstOrDefault(x => x.Definition == definition) as Statistic<T>;
            if (statistic == null)
            {
                value = default(T);
                return false;
            }
            else
            {
                value = statistic.GetValue(context);
                return true;
            }
        }

        public string ParseDescription(object caller)
        {
            string description = this.description;
            foreach (Statistic statistic in statistics)
            {
                description = description.Replace($"{{{statistic.Definition.Title.Replace(" ", "_").ToLower()}}}", $"{statistic.GetDescriptionFormatted(caller)}");
            }

            return description;
        }
    }
}
