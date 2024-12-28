using Game.Components;
using Game.Modifier;
using Game.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            All = new List<Entity>();
        }

        public static List<Entity> All = new List<Entity>();

        public enum EntityTag
        {
            Building,
            Ranger
        }

        [SerializeField] private List<EntityTag> tags = new List<EntityTag>();
        [SerializeField] private StatisticRepository statisticRepository;

        public delegate void OnParentChangedDelegate(Entity entity, Entity oldParent, Entity newParent);
        public event OnParentChangedDelegate OnParentChanged;

        public delegate void OnDestroyDelegate(Entity entity);
        public event OnDestroyDelegate OnDeactivated;

        public delegate void OnDescendantAddedDelegate(Entity entity);
        public event OnDescendantAddedDelegate OnDescendantAdded;

        public Entity Parent
        {
            get
            {
                return parent;
            }
            set
            {
                if (parent != null)
                {
                    this.OnDescendantAdded -= parent.OnDescendantAdded;
                    parent.children.Remove(this);
                }

                OnParentChanged?.Invoke(this, parent, value);
                parent = value;
                if (parent != null)
                {
                    this.OnDescendantAdded += parent.OnDescendantAdded;
                    parent.OnDescendantAdded?.Invoke(this);
                    parent.children.Add(this);
                }
            }
        }
        public IReadOnlyList<Entity> Children => children;
        public virtual bool IsActive { get => true; }
        public List<EntityTag> Tags { get => tags; }
        public virtual Definition Definition { get; set; }
        public Statistic this[StatisticDefinition definition] => StatisticRepository[definition];
        public Statistic this[string name] => StatisticRepository[name];
        public StatisticRepository StatisticRepository { get => statisticRepository; }
        public EventChannelHandler EventChannelHandler { get; set; } = new EventChannelHandler();

        private Dictionary<Type, List<object>> cached = new Dictionary<Type, List<object>>();
        private Entity parent = null;
        private List<Entity> children = new List<Entity>();

        protected virtual void Awake()
        {
            All.Add(this);
            AddOrGetCachedComponent<ModifierApplier>();
            EntityCreatedEventChannel.Global.Publish(new EntityCreatedEventChannel.Event(this));
            Link<StatisticRepository>(statisticRepository);
        }

        public virtual void Initialize()
        {
            StatisticRepository.Initialize(this);
        }

        public virtual void Deactivate()
        {
            Parent = null;
            gameObject.SetActive(false);

            OnDeactivated?.Invoke(this);
            GameObject.Destroy(this.gameObject);
        }

        protected virtual void OnDestroy()
        {
            All.Remove(this);
        }

        #region Components
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
                return (T)cached[typeof(T)].FirstOrDefault();
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
        #endregion
    }

    public abstract class Entity<T> : Entity
        where T : Definition
    {
        protected T definition;

        public override Definition Definition { get => definition; set => definition = value as T; }

        public T GetDefinition()
        {
            return definition;
        }
    }
}