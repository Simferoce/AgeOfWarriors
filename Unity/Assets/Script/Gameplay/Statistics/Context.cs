namespace Game.Statistics
{
    public class Context
    {
        public Entity Source { get; set; }

        public Context(Entity source)
        {
            Source = source;
        }
    }
}
