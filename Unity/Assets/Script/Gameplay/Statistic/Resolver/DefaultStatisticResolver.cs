using System.Reflection;

namespace Game
{
    public class DefaultStatisticResolver : StatisticResolver
    {
        public override bool CanResolve(object current, string path)
        {
            return true;
        }

        public override object Resolve(object current, string path)
        {
            PropertyInfo[] propertyInfos = current.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                StatisticAttribute statisticAttribute = propertyInfo.GetCustomAttribute<StatisticAttribute>();
                if (statisticAttribute != null
                    && path.StartsWith(statisticAttribute.Name))
                {
                    current = propertyInfo.GetValue(current);
                    if (path == statisticAttribute.Name)
                        return current;

                    if (statisticAttribute.Name.Length > 0)
                        path = path.Substring(statisticAttribute.Name.Length);

                    object result = StatisticResolverService.Resolve(current, path);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }
    }
}
