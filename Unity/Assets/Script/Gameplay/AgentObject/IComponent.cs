namespace Game
{
    public interface IComponent
    {
        public bool TryGetCachedComponent<T>(out T component);
        public T GetCachedComponent<T>();

        public T AddOrGetCachedComponent<T>()
            where T : UnityEngine.Component;
    }
}
