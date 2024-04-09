using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticReference<T>
    {
        [SerializeField] private string path;

        public Statistic<T> Get(StatisticContext context)
        {
            string[] node = path.Split('.');
            object current = context.Children[node[0]];
            for (int i = 1; i < node.Length; ++i)
            {
                PropertyInfo propertyInfo = current
                    .GetType()
                    .GetProperties()
                    .Where(x => Match(x, node[i]))
                    .FirstOrDefault();

                Debug.Assert(propertyInfo != null, $"Could not resolve {path} - {node[i]} from {current.GetType().Name}");

                current = propertyInfo.GetValue(current);
            }

            return (Statistic<T>)current;
        }

        public bool Match(PropertyInfo info, string name)
        {
            StatisticResolveAttribute statisticResolveAttribute = info.GetAttribute<StatisticResolveAttribute>();
            if (statisticResolveAttribute == null)
                return false;

            return statisticResolveAttribute.Name == name;
        }
    }

    public class StatisticContext
    {
        public Dictionary<string, object> Children { get; set; }

        public StatisticContext(params (string key, object context)[] children)
        {
            Children = new Dictionary<string, object>(children.ToDictionary(x => x.key, x => x.context));
        }
    }
}
