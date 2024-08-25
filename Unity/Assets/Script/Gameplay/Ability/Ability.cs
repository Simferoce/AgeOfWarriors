using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Ability : MonoBehaviour, IStatisticProvider
    {
        public Caster Caster { get; set; }
        public abstract float Cooldown { get; }

        public event System.Action OnAbilityEffectApplied;
        public event System.Action<Ability> OnAbilityUsed;

        public bool IsCasting { get; set; }
        public virtual bool IsActive => IsCasting;
        public virtual List<ITargeteable> Targets => new List<ITargeteable>();
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

        public virtual bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic)
        {
            if (path.SequenceEqual("caster"))
            {
                statistic = StatisticUtility.ConvertGeneric<T, Caster>(Caster);
                return true;
            }
            else if (path.SequenceEqual("cooldown"))
            {
                statistic = StatisticUtility.ConvertGeneric<T, float>(Cooldown);
                return true;
            }
            else if (path.StartsWith("caster"))
            {
                path = path.Slice("caster".Length + 1);
                return Caster.AgentObject.TryGetStatistic(path, out statistic);
            }
            else
            {
                statistic = default;
                return false;
            }
        }
    }

    public abstract class Ability<T> : Ability
        where T : AbilityDefinition
    {
        protected T definition { get => Definition as T; set => Definition = value; }
        public override string ParseDescription() => Definition.ParseDescription();
    }
}
