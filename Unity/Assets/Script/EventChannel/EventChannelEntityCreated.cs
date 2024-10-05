namespace Game
{
    public class EventChannelEntityCreated : EventChannel<EventChannelEntityCreated.Event>
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
