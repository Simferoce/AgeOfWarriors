namespace Game
{
    public class EventChannelDeath : EventChannel<EventChannelDeath.Event>
    {
        public class Event
        {
            public AgentObject AgentObject;
        }
    }
}
