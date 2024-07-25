using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Ability : MonoBehaviour
    {
        public Character Character { get; set; }
        [Statistic("cooldown")] public abstract float Cooldown { get; }

        public event System.Action OnAbilityEffectApplied;
        public bool IsCasting { get; set; }
        public virtual bool IsActive => IsCasting;
        public virtual List<ITargeteable> Targets => new List<ITargeteable>();
        public AbilityDefinition Definition { get; set; }
        public abstract string ParseDescription();
        public Faction FactionWhenUsed { get; set; }

        public virtual void Initialize(Character character)
        {
            this.Character = character;
        }

        public abstract void Dispose();

        public abstract void Tick();

        public abstract bool CanUse();

        public abstract void Use();

        public abstract void Apply();

        public abstract void Interrupt();

        protected void PublishEffectApplied()
        {
            OnAbilityEffectApplied?.Invoke();
        }
    }

    public abstract class Ability<T> : Ability
        where T : AbilityDefinition
    {
        protected T definition { get => Definition as T; set => Definition = value; }
        public override string ParseDescription() => Definition.ParseDescription();
    }
}
