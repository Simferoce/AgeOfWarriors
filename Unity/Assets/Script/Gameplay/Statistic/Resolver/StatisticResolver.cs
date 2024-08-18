namespace Game
{
    public abstract class StatisticResolver
    {
        public abstract bool CanResolve(object current, string path);
        public abstract object Resolve(object current, string path);
    }
}
