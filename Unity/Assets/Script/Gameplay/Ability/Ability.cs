using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [StatisticClass("ability")]
    public abstract partial class Ability : MonoBehaviour
    {
        [Statistic("caster")] public Caster Caster { get; set; }
        public virtual float Cooldown { get; }

        public event System.Action OnAbilityEffectApplied;
        public event System.Action<Ability> OnAbilityUsed;

        public bool IsCasting { get; set; }
        public virtual bool IsActive => IsCasting;
        public virtual List<Target> Targets => new List<Target>();
        public AbilityDefinition Definition { get; set; }
        public abstract string ParseDescription();
        public Faction FactionWhenUsed { get; set; }

        public string StatisticProviderName => "ability";

        public virtual void Initialize(Caster caster)
        {
            this.Caster = caster;
        }

        public abstract void Dispose();

        public abstract void Tick();

        public abstract bool CanUse();

        public virtual void Use()
        {
            InternalUse();
            OnAbilityUsed?.Invoke(this);
        }

        public abstract void InternalUse();

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
