using System;
using System.Collections.Generic;

namespace AgeOfWarriors.Core
{
    public class EventChannel
    {
        private Dictionary<Type, List<object>> subscribers = new Dictionary<Type, List<object>>();

        public void Subscribe<T>(System.Action<T> subscriber)
        {
            if (!subscribers.ContainsKey(typeof(T)))
                subscribers.Add(typeof(T), new List<object>());

            subscribers[typeof(T)].Add(subscriber);
        }

        public void Unsubscribe<T>(System.Action<T> subscriber)
        {
            if (!subscribers.ContainsKey(typeof(T)))
                subscribers.Add(typeof(T), new List<object>());

            subscribers[typeof(T)].Remove(subscriber);
        }

        public void Publish<T>(T data)
        {
            if (!subscribers.ContainsKey(typeof(T)))
                return;

            foreach (object action in subscribers[typeof(T)])
            {
                (action as System.Action<T>).Invoke(data);
            }
        }
    }
}
