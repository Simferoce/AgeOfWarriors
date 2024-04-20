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
        [SerializeField] private bool showOnHealthBar = true;
        [SerializeReference] private List<Statistic> statistics;

        public Sprite Icon { get => icon; }
        public string Title { get => title; }
        public string Description { get => description; }
        public bool Show { get => showOnHealthBar; set => showOnHealthBar = value; }

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

        public string ParseDescription(object caller)
        {
            return ParseDescription(caller, this.description);
        }

        public virtual string ParseDescription(object caller, string description)
        {
            foreach (Statistic statistic in statistics)
            {
                description = description.Replace($"{{val:{statistic.Definition.HumanReadableId}}}", statistic.GetDescriptionFormatted(caller));
            }

            description = StatisticDefinition.ParseText(description);

            return description;
        }
    }
}
