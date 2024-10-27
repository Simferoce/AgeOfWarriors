using Game.Components;
using Game.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
#endif
using UnityEngine;

namespace Game.Ability
{
    [RequireComponent(typeof(AttackFactory))]
    public abstract class AbilityEntity : Entity
    {
        [SerializeReference, SubclassSelector] protected List<AbilityCondition> conditions = new List<AbilityCondition>();

        public event Action<AbilityEntity> OnAbilityUsed;
        public event Action OnAbilityEffectApplied;

        public Caster Caster { get; set; }
        public bool IsCasting { get; set; } = false;
        public virtual List<Target> Targets => new List<Target>();
        public AbilityDefinition Definition { get; set; }
        public override FactionType Faction => faction;
        public override bool IsActive => base.IsActive && Caster.enabled;

        protected FactionType faction;

        public virtual void Initialize(Caster caster)
        {
            GetCachedComponent<StatisticRegistry>().Add(caster.Entity.GetCachedComponent<StatisticRegistry>());

            Caster = caster;
            Parent = caster.Entity;
            faction = caster.Entity.Faction;

            base.Initialize();

            foreach (AbilityCondition condition in conditions)
                condition.Initialize(this);
        }

        public abstract void Dispose();

        public abstract void Tick();

        public virtual bool CanUse()
        {
            return conditions.All(x => x.Execute());
        }

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

        public string ParseDescription()
        {
            return Definition.ParseDescription(this, null);
        }
    }
}
