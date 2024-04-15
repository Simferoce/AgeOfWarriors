using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Entity : MonoBehaviour
    {
        private Dictionary<Type, object> cached = new Dictionary<Type, object>();

        public bool TryGetCachedComponent<T>(out T component)
        {
            component = GetCachedComponent<T>();
            return component != null;
        }

        public T GetCachedComponent<T>()
        {
            if (cached.ContainsKey(typeof(T)))
            {
                return (T)cached[typeof(T)];
            }
            else
            {
                cached[typeof(T)] = GetComponent<T>();
                return (T)cached[typeof(T)];
            }
        }
    }
}
