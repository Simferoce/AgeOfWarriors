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
        public virtual float Cooldown => GetStatistic().FirstOrDefault(x => x.Name == "cooldown")?.GetValueOrDefault<float>() ?? 0f;

        public event Action<Ability> OnAbilityUsed;

        public event Action OnAbilityEffectApplied;
        public bool IsCasting { get; set; }
        public virtual bool IsActive => IsCasting;
        public virtual List<Target> Targets => new List<Target>();
        public AbilityDefinition Definition { get; set; }
        public Faction FactionWhenUsed { get; set; }

        public virtual void Initialize(Caster caster)
        {
            Caster = caster;

            foreach (Statistic statistic in statistics)
                statistic.Initialize(this);
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

        public virtual IEnumerable<Statistic> GetStatistic()
        {
            yield return new StatisticTemporary<Caster>(this, "caster", Caster);

            foreach (Statistic statistic in statistics)
                yield return statistic;

            yield break;
        }

        public bool IsName(ReadOnlySpan<char> name)
        {
            return name.SequenceEqual("ability");
        }

        public string ParseDescription()
        {
            return Definition.ParseDescription();
        }
    }
}
