using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticHolder
    {
        [SerializeReference, SerializeReferenceDropdown] private List<Statistic> statistics = new List<Statistic>();

        public Statistic Get(string title)
        {
            return statistics.FirstOrDefault(x => x.Title.ToLower() == title.ToLower());
        }
    }
}
