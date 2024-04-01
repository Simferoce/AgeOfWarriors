using System;
using System.Collections.Generic;

namespace Game
{
    public abstract class Modifier : IModifier, IDisposable
    {
        public List<CharacterModifierElement> modifierElements = new List<CharacterModifierElement>();

        protected IModifiable modifiable;

        protected Modifier(IModifiable modifiable)
        {
            this.modifiable = modifiable;
        }

        public virtual float? SpeedPercentage => null;
        public virtual float? Defense => null;

        public virtual void Update()
        {
            bool end = false;
            foreach (CharacterModifierElement element in modifierElements)
            {
                end |= element.Update();
            }

            if (end)
                modifiable.RemoveModifier(this);
        }

        public virtual void Refresh()
        {
            foreach (CharacterModifierElement element in modifierElements)
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

        public T With(List<CharacterModifierElement> modifierElements)
        {
            this.modifierElements.AddRange(modifierElements);
            return (T)this;
        }

        public T With(CharacterModifierElement modifierElement)
        {
            this.modifierElements.Add(modifierElement);
            return (T)this;
        }
    }
}
