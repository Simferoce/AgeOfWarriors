using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Ability : MonoBehaviour
    {
        public Character Character { get; set; }
        public AbilityDefinition Definition { get; set; }

        public bool IsCasting { get; set; }
        public virtual bool IsActive => IsCasting;
        public virtual List<ITargeteable> Targets => new List<ITargeteable>();

        public virtual string ParseDescription() => Definition.ParseDescription();

        public virtual void Initialize(Character character)
        {
            this.Character = character;
        }

        public abstract void Dispose();

        public abstract void Tick();

        public abstract bool CanUse();

        public abstract void Use();

        public abstract void Interrupt();
    }
}
