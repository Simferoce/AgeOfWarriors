using System.Collections.Generic;

namespace Game
{
    public class EventChannel<T>
    {
        public delegate void EventChannelHandler(T evt);

        private static EventChannel<T> instance = null;
        public static EventChannel<T> Instance
        {
            get
            {
                if (instance == null)
                    instance = new EventChannel<T>();

                return instance;
            }
        }

        private List<EventChannelHandler> subscribers = new List<EventChannelHandler>();

        public void Susbribe(EventChannelHandler subscriber)
        {
            subscribers.Add(subscriber);
        }

        public void Unsubcribe(EventChannelHandler subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public void Publish(T evt)
        {
            foreach (EventChannelHandler handler in subscribers)
            {
                handler?.Invoke(evt);
            }
        }
    }
}
