using System;
using System.Collections.Generic;
using System.Linq;
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
                if (current is StatisticHolder holder)
                {
                    current = holder.Get(node[i]);
                }
            }

            Statistic<T> statistic = (Statistic<T>)current;
            Debug.Assert(statistic != null, $"Could not resolve {path}");

            return statistic;
        }
    }

    public class StatisticContext
    {
        public Dictionary<string, StatisticHolder> Children { get; set; }

        public StatisticContext(params (string key, StatisticHolder context)[] children)
        {
            Children = new Dictionary<string, StatisticHolder>(children.ToDictionary(x => x.key, x => x.context));
        }
    }
}
