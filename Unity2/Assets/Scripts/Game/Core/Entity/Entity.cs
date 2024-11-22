using System.Collections.Generic;
using System.Linq;

namespace AgeOfWarriors
{
    public abstract class Entity
    {
        public Game Game { get => game; }

        public virtual float this[string name]
        {
            get
            {
                if (!statistics.ContainsKey(name))
                {
                    game.Debug.LogError($"Could not find the statistic \"{name}\" in \"{this}\"");
                    return default;
                }

                return statistics[name];
            }
            set
            {
                statistics[name] = value;
            }
        }

        protected Dictionary<string, float> statistics = new Dictionary<string, float>();
        private List<Component> components = new List<Component>();
        private Game game;

        public Entity(Game game)
        {
            this.game = game;
        }

        public Entity(Game game, List<Component> components)
            : this(game)
        {
            this.components = components;
        }

        public virtual void Initialize()
        {
            game.Entities.Add(this);
            game.EventChannel.Publish(new EntityCreatedEvent(this));
        }

        public virtual void Update() { }

        public void AddComponent(Component component)
        {
            components.Add(component);
        }

        public void RemoveComponent(Component component)
        {
            components.Remove(component);
        }

        public bool TryGetComponent<T>(out T component)
            where T : Component
        {
            component = components.FirstOrDefault(x => x is T) as T;
            return component != null;
        }

        public T GetComponent<T>()
            where T : Component
        {
            return components.FirstOrDefault(x => x is T) as T;
        }
    }
}
