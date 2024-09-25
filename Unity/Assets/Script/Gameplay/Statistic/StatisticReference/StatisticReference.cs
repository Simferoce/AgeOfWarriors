using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    [Serializable]
    public class StatisticReference : Statistic
    {
        [SerializeReference, SubclassSelector] StatisticQuery query;

        public override string Name { get => Resolve()?.Name ?? "Undefined"; set => Resolve().Name = value; }
        public override StatisticDefinition Definition { get => Resolve()?.Definition; set => Resolve().Definition = value; }

        public Statistic Resolve()
        {
            Assert.IsTrue(context != null, "The statistic reference has not been initialized.");

            return query?.GetStatistics(context).FirstOrDefault();
        }

        public override bool TryGetValue<T>(out T value)
        {
            value = default;

            return Resolve()?.TryGetValue(out value) ?? false;
        }
    }
}
