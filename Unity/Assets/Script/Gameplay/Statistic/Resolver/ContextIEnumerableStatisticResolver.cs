using System.Collections.Generic;
using System.Reflection;

namespace Game
{
    public class ContextIEnumerableStatisticResolver : StatisticResolver
    {
        public override bool CanResolve(object current, string path)
        {
            return current is IEnumerable<IContext>;
        }

        public override object Resolve(object current, string path)
        {
            if (current is not IEnumerable<IContext> contexts)
                throw new System.Exception($"Unexpected exception. The type of the object should have been {nameof(IEnumerable<IContext>)}. (Should not happens)");

            foreach (IContext context in contexts)
            {
                foreach (StatisticObjectAttribute statisticObject in context.GetType().GetCustomAttributes<StatisticObjectAttribute>())
                {
                    if (path.StartsWith(statisticObject.Name))
                    {
                        if (path == statisticObject.Name)
                            return context;

                        if (statisticObject.Name.Length > 0)
                            path = path.Substring(statisticObject.Name.Length + 1);

                        object result = StatisticResolverService.Resolve(context, path);
                        if (result != null)
                            return result;
                    }
                }
            }

            return null;
        }
    }
}
