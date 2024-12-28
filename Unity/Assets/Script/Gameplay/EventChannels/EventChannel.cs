using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EventChannel
    {
    }

    public class EventChannel<T> : EventChannel
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            global = null;
        }

        public delegate void EventChannelDelegate(T evt);

        private static EventChannel<T> global = null;
        public static EventChannel<T> Global
        {
            get
            {
                if (global == null)
                    global = new EventChannel<T>();

                return global;
            }
        }

        private List<EventChannelDelegate> subscribers = new List<EventChannelDelegate>();

        public void Subscribe(EventChannelDelegate subscriber)
        {
            subscribers.Add(subscriber);
        }

        public void Unsubscribe(EventChannelDelegate subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public void Publish(T evt)
        {
            foreach (EventChannelDelegate handler in subscribers)
            {
                handler?.Invoke(evt);
            }
        }
    }
}
