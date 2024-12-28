using System;
using System.Collections.Generic;

namespace Game.Components
{
    public class EventChannelHandler
    {
        private Dictionary<Type, EventChannel> channels = new Dictionary<Type, EventChannel>();

        public void Subscribe<T>(EventChannel<T>.EventChannelDelegate eventChannelDelegate)
        {
            if (!channels.ContainsKey(typeof(T)))
                channels[typeof(T)] = new EventChannel<T>();

            (channels[typeof(T)] as EventChannel<T>).Subscribe(eventChannelDelegate);
        }

        public void Unsubscribe<T>(EventChannel<T>.EventChannelDelegate eventChannelDelegate)
        {
            if (!channels.ContainsKey(typeof(T)))
                channels[typeof(T)] = new EventChannel<T>();

            (channels[typeof(T)] as EventChannel<T>).Unsubscribe(eventChannelDelegate);
        }

        public void Publish<T>(T data)
        {
            if (!channels.ContainsKey(typeof(T)))
                channels[typeof(T)] = new EventChannel<T>();

            (channels[typeof(T)] as EventChannel<T>).Publish(data);
            EventChannel<T>.Global.Publish(data);
        }
    }
}
