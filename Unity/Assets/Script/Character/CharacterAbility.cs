using System;

namespace Game
{
    [Serializable]
    public abstract class CharacterAbility : IDisposable
    {
        public bool IsCasting { get; set; }

        protected Character character;

        public virtual void Initialize(Character character)
        {
            this.character = character;
        }

        public virtual void Dispose()
        {
        }

        public abstract bool CanUse();

        public abstract void Use();
    }
}
