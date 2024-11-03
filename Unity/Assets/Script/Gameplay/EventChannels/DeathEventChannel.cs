namespace Game.EventChannel
{
    public class DeathEventChannel : EventChannel<DeathEventChannel.Event>
    {
        public class Event
        {
            public Entity Entity { get; set; }
        }
    }
}
