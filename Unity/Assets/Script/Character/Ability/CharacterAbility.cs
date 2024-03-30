using System;

namespace Game
{
    [Serializable]
    public abstract class CharacterAbility
    {
        protected Character character;

        public bool IsCasting { get; set; }
        public virtual bool IsActive => IsCasting;

        public virtual void Initialize(Character character)
        {
            this.character = character;
        }

        public abstract void Dispose();

        public abstract void Update();

        public abstract bool CanUse();

        public abstract void Use();
    }
}
