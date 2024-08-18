using System.Collections.Generic;

namespace Game
{
    public static class StatisticResolverService
    {
        private static List<StatisticResolver> RESOLVERS = new List<StatisticResolver>()
        {
            new ContextIEnumerableStatisticResolver(),
            new DefaultStatisticResolver(),
        };

        public static object Resolve(object current, string path)
        {
            foreach (StatisticResolver resolver in RESOLVERS)
            {
                if (resolver.CanResolve(current, path))
                    return resolver.Resolve(current, path);
            }

            return null;
        }
    }
}
