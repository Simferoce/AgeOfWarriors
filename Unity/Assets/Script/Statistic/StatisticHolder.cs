using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticHolder
    {
        [Serializable]
        public class Reference
        {
            [SerializeField] private string name;

            public string Name { get => name; set => name = value; }
            public Func<StatisticHolder> Function { get; set; }
        }

        [Serializable]
        public class StatisticReference
        {
            [SerializeField] private string name;
            [SerializeReference, SerializeReferenceDropdown] private Statistic statistic;

            public string Name { get => name; set => name = value; }
            public Statistic Statistic { get => statistic; set => statistic = value; }
        }

        [SerializeField] List<Reference> references = new List<Reference>();
        [Space]
        [SerializeField] private List<StatisticReference> statistics = new List<StatisticReference>();

        public void Initialize(object owner)
        {
            foreach (StatisticReference statisticReference in statistics)
            {
                statisticReference.Statistic.Initialize(owner);
            }
        }

        public object Get(string name)
        {
            object value = GetStatistic(name);
            if (value == null)
                value = GetReference(name);

            return value;
        }

        public Statistic GetStatistic(string name)
        {
            return statistics.FirstOrDefault(x => x.Name.ToLower() == name.ToLower())?.Statistic;
        }

        public object GetReference(string name)
        {
            return references.FirstOrDefault(x => x.Name.ToLower() == name.ToLower())?.Function?.Invoke();
        }

        public void DefineReference(string name)
        {
            name = name.ToLower();

            Reference reference = references.FirstOrDefault(x => x.Name.ToLower() == name);
            if (reference == null)
                references.Add(new Reference() { Name = name, Function = null });
        }

        public void SetReference(string name, Func<StatisticHolder> function)
        {
            name = name.ToLower();

            Reference reference = references.FirstOrDefault(x => x.Name.ToLower() == name);
            if (reference != null)
                reference.Function = function;
            else
                references.Add(new Reference() { Name = name, Function = function });
        }

        public void DefineStatistic(string name, Statistic statistic)
        {
            name = name.ToLower();

            StatisticReference reference = statistics.FirstOrDefault(x => x.Name == name);
            if (reference == null)
                statistics.Add(new StatisticReference() { Name = name, Statistic = statistic });
        }
    }
}
