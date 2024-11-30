namespace Game
{
    public class DeathEventChannel : EventChannel<DeathEventChannel.Event>
    {
        public class Event
        {
            public AgentObject AgentObject;
        }
    }
}
