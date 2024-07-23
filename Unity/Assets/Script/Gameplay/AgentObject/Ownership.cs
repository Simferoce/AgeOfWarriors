using System;
using System.Collections.Generic;

namespace Game
{
    public class Ownership : Entity
    {
        public List<Ownership> Children { get; } = new List<Ownership>();

        public event Action<Ownership> OnChildAdded;
        public Ownership Owner { get; private set; }

        public static void SetOwner(Entity of, Entity owner)
        {
            of.AddOrGetCachedComponent<Ownership>().SetOwner(owner.AddOrGetCachedComponent<Ownership>());
        }

        public void SetOwner(Ownership owner)
        {
            if (Owner != null)
                Owner.RemoveChild(this);

            Owner = owner;

            if (Owner != null)
                Owner.AddChild(this);
        }

        public void AddChild(Ownership owneable)
        {
            Children.Add(owneable);
            OnChildAdded?.Invoke(owneable);
        }

        public void RemoveChild(Ownership owneable)
        {
            Children.Remove(owneable);
        }

        private void OnDestroy()
        {
            if (Owner != null)
                Owner.RemoveChild(this);
        }
    }
}
