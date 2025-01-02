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
    public abstract class AbilityEntity : Entity<AbilityDefinition>, ICooldown
    {
        [SerializeReference, SubclassSelector] protected List<AbilityCondition> conditions = new List<AbilityCondition>();

        public event Action<AbilityEntity> OnAbilityUsed;
        public event Action OnAbilityEffectApplied;

        public Caster Caster { get; set; }
        public bool IsCasting { get; set; } = false;
        public virtual List<Target> Targets => new List<Target>();
        public override bool IsActive => base.IsActive && Caster.enabled;
        public FactionType Faction { get; set; }
        public float Remaining => (conditions.FirstOrDefault(x => x is ICooldown) as ICooldown)?.Remaining ?? 0f;
        public float Total => (conditions.FirstOrDefault(x => x is ICooldown) as ICooldown)?.Total ?? 0f;

        public virtual void Initialize(Caster caster)
        {
            Caster = caster;
            Parent = caster.Entity;
            Faction = caster.Entity["faction"].Get<FactionType>();

            base.Initialize();

            foreach (AbilityCondition condition in conditions)
                condition.Initialize(this);

            StatisticRepository.Add(new Statistic<FactionType>("faction", null, new SerializeValue<FactionType>(), (FactionType baseValue) => caster.Entity["faction"].Get<FactionType>()));
            StatisticRepository.Add(new StatisticFloat("flat_damage_versus_weak", StatisticDefinitionRegistry.Instance.FlatDamageVersusWeak, 0f, (float baseValue) => baseValue + caster.Entity[StatisticDefinitionRegistry.Instance.FlatDamageVersusWeak]));
        }

        public abstract void Dispose();

        public virtual void Tick()
        {
        }

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
            return definition.ParseDescription(this);
        }
    }
}
