using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class StatisticReference : Statistic
    {
        [SerializeField] private string path;

        public StatisticReference()
        {

        }

        public StatisticReference(string path)
        {
            this.path = path;
        }

        public override StatisticDefinition GetDefinition(IStatisticContext context) => StatisticUtility.Resolve(context, path).GetDefinition(context);
        public override string GetName(IStatisticContext context) => StatisticUtility.Resolve(context, path).GetName(context);
        public override string SetName(IStatisticContext context, string value) => StatisticUtility.Resolve(context, path).SetName(context, value);

        public override bool TryGetValue<T>(IStatisticContext context, out T value)
        {
            return StatisticUtility.TryResolveValue<T>(context, path, out value);
        }
    }
}
