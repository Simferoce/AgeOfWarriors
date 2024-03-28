using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class CharacterAbility : IDisposable
    {
        public bool IsCasting { get; set; }

        protected Character character;
        private float lastUsed = 0f;

        public virtual void Initialize(Character character)
        {
            this.character = character;
        }

        public virtual void Dispose()
        {
        }

        public virtual bool CanUse()
        {
            return Time.time - lastUsed > character.AttackPerSeconds;
        }

        public virtual void Use()
        {
            lastUsed = Time.time;
        }
    }
}
