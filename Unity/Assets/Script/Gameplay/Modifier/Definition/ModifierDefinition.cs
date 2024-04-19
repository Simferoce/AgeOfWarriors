using System;
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
        [SerializeReference] private List<Statistic> statistics;

        public Sprite Icon { get => icon; }
        public string Title { get => title; }
        public string Description { get => description; }

        public T GetValueOrThrow<T>(object context, StatisticDefinition definition)
        {
            return TryGetValue<T>(context, definition, out T value) == true ? value : throw new Exception($"Could not resolve the statistic {definition}");
        }

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

        public virtual string ParseDescription(object caller)
        {
            string description = this.description;
            foreach (Statistic statistic in statistics)
            {
                description = description.Replace($"{{val:{statistic.Definition.HumanReadableId}}}", statistic.GetDescriptionFormatted(caller));
            }

            description = StatisticDefinition.ParseText(description);

            return description;
        }
    }
}
