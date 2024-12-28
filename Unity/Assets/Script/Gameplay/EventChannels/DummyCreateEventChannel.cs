namespace Game
{
    public class DummyCreateEventChannel : EventChannel<DummyCreateEventChannel.Event>
    {
        public class Event
        {
            public Entity Entity { get; set; }

            public Event(Entity entity)
            {
                Entity = entity;
            }
        }
    }
}
