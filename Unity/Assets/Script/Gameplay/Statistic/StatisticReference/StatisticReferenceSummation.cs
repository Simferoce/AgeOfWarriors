using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    [Serializable]
    public class StatisticReferenceSummation : Statistic
    {
        [SerializeReference, SubclassSelector] StatisticQuery query;

        public override string Name { get => Resolve().FirstOrDefault()?.Name ?? "Undefined"; set => throw new NotImplementedException(); }
        public override StatisticDefinition Definition { get => Resolve().FirstOrDefault()?.Definition; set => throw new NotImplementedException(); }

        public StatisticReferenceSummation()
        {

        }

        public StatisticReferenceSummation(StatisticQuery query)
        {
            this.query = query;
        }

        public IEnumerable<Statistic> Resolve()
        {
            Assert.IsTrue(context != null, "The statistic reference has not been initialized.");

            return query?.GetStatistics(context);
        }

        public override bool TryGetValue<T>(out T value)
        {
            value = default;

            float result = 0f;
            foreach (Statistic statistic in Resolve())
                result += statistic.GetValueOrDefault<float>();

            value = StatisticUtility.ConvertGeneric<T, float>(result);
            return true;
        }
    }
}
