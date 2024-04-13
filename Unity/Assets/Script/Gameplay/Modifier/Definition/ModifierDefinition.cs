using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class ModifierDefinition : Definition
    {
        [Serializable]
        private class StatisticDescriptionReference
        {
            [SerializeField] private string key;
            [SerializeField] private StatisticDefinition definition;

            public string Key => key;
            public StatisticDefinition Definition => definition;
        }

        [SerializeField] private string title;
        [SerializeField] private Sprite icon;
        [SerializeField] private List<StatisticDescriptionReference> definitions;
        [SerializeField] private string description;

        public Sprite Icon { get => icon; }
        public string Title { get => title; }
        public string Description { get => description; }

        public string ParseDescription(Modifier modifier)
        {
            string description = this.description;
            foreach (StatisticDescriptionReference definition in definitions)
            {
                if (!modifier.TryGetStatistic<object>(definition.Definition, out object value))
                    throw new System.Exception($"Could not find the statistic {definition.Definition.Name} for {modifier.ToString()}");

                description = description.Replace($"{{{definition.Key.ToLower()}}}", $"{value}");
            }

            return description;
        }
    }
}
