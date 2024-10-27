using Game.EventChannel;
using Game.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        [SerializeField, FormerlySerializedAs("index")] private StatisticRegistry statisticRegistry;

        public delegate void OnParentChangedDelegate(Entity entity, Entity oldParent, Entity newParent);

        public event OnParentChangedDelegate OnParentChanged;

        public Entity Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (parent != null)
                    parent.children.Remove(this);

                OnParentChanged?.Invoke(this, parent, value);
                parent = value;

                if (parent != null)
                    parent.children.Add(this);
            }
        }
        public IReadOnlyList<Entity> Children => children;
        public virtual FactionType Faction => FactionType.Undefined;
        public virtual bool IsActive { get => true; }

        private Dictionary<Type, List<object>> cached = new Dictionary<Type, List<object>>();
        private Entity parent = null;
        private List<Entity> children = new List<Entity>();

        protected virtual void Awake()
        {
            EntityRepository.Instance.Add(this);
            Link(statisticRegistry);
            statisticRegistry.Initialize(this);
            EntityCreatedEventChannel.Instance.Publish(new EntityCreatedEventChannel.Event(this));
        }

        protected virtual void OnDestroy()
        {
            EntityRepository.Instance.Remove(this);
        }

        public void Link<T>(T component)
            where T : class
        {
            if (!cached.ContainsKey(typeof(T)))
                cached[typeof(T)] = new List<object>();

            cached[typeof(T)].Add(component);
        }

        public bool TryGetCachedComponent<T>(out T component)
            where T : class
        {
            component = GetCachedComponent<T>() as T;
            return component != null;
        }

        public T GetCachedComponent<T>()
            where T : class
        {
            if (cached.ContainsKey(typeof(T)))
            {
                return (T)cached[typeof(T)].First();
            }
            else
            {
                if (typeof(Component).IsAssignableFrom(typeof(T)))
                    cached[typeof(T)] = GetComponentsInChildren<T>().Cast<object>().ToList();
                else
                    cached[typeof(T)] = new List<object>();

                return (T)cached[typeof(T)].FirstOrDefault();
            }
        }

        public T AddOrGetCachedComponent<T>()
            where T : class
        {
            T component = GetCachedComponent<T>();
            if (component != null)
                return component;

            if (typeof(Component).IsAssignableFrom(typeof(T)))
                component = gameObject.AddComponent(typeof(T)) as T;
            else
                component = Activator.CreateInstance(typeof(T)) as T;

            cached[typeof(T)] = new List<object>() { component };
            return component;
        }

        public IEnumerable<Entity> GetHierarchy()
        {
            Entity current = this;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }
    }
}