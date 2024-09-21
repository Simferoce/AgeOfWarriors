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

        public override bool TryGetValue<T>(IStatisticContext context, out T value)
        {
            value = default;

            ReadOnlySpan<char> span = path.AsSpan();
            int start = 0;
            int index;

            IStatisticContext current = context;
            while ((index = span.Slice(start).IndexOf(".")) != -1)
            {
                current = GetContext(context, span.Slice(start, index));
                if (current == null)
                    return false;

                start += index + 1;
            }

            return current.GetStatistic(span.Slice(start))?.TryGetValue(current, out value) ?? false;
        }

        private IStatisticContext GetContext(IStatisticContext context, ReadOnlySpan<char> part)
        {
            return context.GetContext(part);
        }
    }
}
