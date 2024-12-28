namespace Game
{
    public class EntityCreatedEventChannel : EventChannel<EntityCreatedEventChannel.Event>
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
