using System;
using System.Collections.Generic;

namespace Game
{
    public abstract class Modifier : IModifier, IDisposable
    {
        public List<ModifierElement> modifierElements = new List<ModifierElement>();

        public virtual float? SpeedPercentage => null;
        public virtual float? Defense => null;
        public virtual float? MaxHealth => null;
        public virtual float? AttackPower => null;
        public virtual bool? Invulnerable => null;

        protected IModifiable modifiable;

        protected Modifier(IModifiable modifiable)
        {
            this.modifiable = modifiable;
        }

        public void Initialize()
        {
            foreach (ModifierElement element in modifierElements)
                element.Initialize();
        }

        public virtual void Update()
        {
            bool end = false;
            foreach (ModifierElement element in modifierElements)
            {
                end |= element.Update();
            }

            if (end)
                modifiable.RemoveModifier(this);
        }

        public virtual void Refresh()
        {
            foreach (ModifierElement element in modifierElements)
            {
                element.Refresh();
            }
        }

        public virtual void Dispose() { }
    }

    public abstract class Modifier<T> : Modifier
        where T : Modifier<T>
    {
        protected Modifier(IModifiable modifiable) : base(modifiable)
        {
        }

        public T With(List<ModifierElement> modifierElements)
        {
            this.modifierElements.AddRange(modifierElements);
            return (T)this;
        }

        public T With(ModifierElement modifierElement)
        {
            this.modifierElements.Add(modifierElement);
            return (T)this;
        }
    }
}
