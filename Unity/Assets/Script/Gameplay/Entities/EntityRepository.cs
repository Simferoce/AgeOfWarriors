using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class EntityRepository : Manager<EntityRepository>
    {
        private List<Entity> entities = new List<Entity>();

        public override IEnumerator InitializeAsync()
        {
            yield break;
        }

        public IEnumerable<T> GetByType<T>()
            where T : Entity
        {
            return entities.OfType<T>();
        }

        public void Add(Entity entity)
        {
            entities.Add(entity);
        }

        public void Remove(Entity entity)
        {
            entities.Remove(entity);
        }
    }
}
