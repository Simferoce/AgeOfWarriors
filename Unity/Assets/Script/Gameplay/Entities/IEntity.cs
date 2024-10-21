namespace Game
{
    public interface IEntity
    {
        public bool TryGetCachedComponent<T>(out T component)
            where T : class
        {
            component = GetCachedComponent<T>() as T;
            return component != null;
        }

        public T GetCachedComponent<T>() where T : class;
    }
}
