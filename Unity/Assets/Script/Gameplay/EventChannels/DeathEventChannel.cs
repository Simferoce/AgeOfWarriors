using Game.Agent;

namespace Game.EventChannel
{
    public class DeathEventChannel : EventChannel<DeathEventChannel.Event>
    {
        public class Event
        {
            public AgentObject AgentObject;
        }
    }
}
