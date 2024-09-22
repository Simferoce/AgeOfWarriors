using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public abstract class Ability : MonoBehaviour, IStatisticContext
    {
        [SerializeReference, SubclassSelector] private List<Statistic> statistics;

        public Caster Caster { get; set; }
        public virtual float Cooldown => GetStatistic("cooldown") ?? 0f;

        public event Action OnAbilityEffectApplied;
        public event Action<Ability> OnAbilityUsed;

        public bool IsCasting { get; set; }
        public virtual bool IsActive => IsCasting;
        public virtual List<Target> Targets => new List<Target>();
        public AbilityDefinition Definition { get; set; }
        public abstract string ParseDescription();
        public Faction FactionWhenUsed { get; set; }

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

        public virtual Statistic GetStatistic(ReadOnlySpan<char> value)
        {
            foreach (Statistic statistic in statistics)
            {
                if (value.SequenceEqual(statistic.Name))
                    return statistic;
            }

            return null;
        }

        public virtual IStatisticContext GetContext(ReadOnlySpan<char> value)
        {
            if (value.SequenceEqual("caster"))
                return Caster;

            return null;
        }

        public bool IsName(ReadOnlySpan<char> name)
        {
            return name.SequenceEqual("ability");
        }
    }

    public abstract class Ability<T> : Ability
        where T : AbilityDefinition
    {
        protected T definition { get => Definition as T; set => Definition = value; }
        public override string ParseDescription() => Definition.ParseDescription();
    }
}
