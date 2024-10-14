using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(AttackFactory))]
    public abstract class Ability : Entity
    {
        [SerializeReference, SubclassSelector] protected List<AbilityCondition> conditions = new List<AbilityCondition>();

        public event Action<Ability> OnAbilityUsed;
        public event Action OnAbilityEffectApplied;

        public Caster Caster { get; set; }
        public float Cooldown => conditions.OfType<CooldownCondition>().Select(x => x.Cooldown).Max();
        public bool IsCasting { get; set; }
        public override bool IsActive => IsCasting;
        public virtual List<Target> Targets => new List<Target>();
        public AbilityDefinition Definition { get; set; }
        public override Faction Faction => faction;

        protected Faction faction;

        public virtual void Initialize(Caster caster)
        {
            Caster = caster;
            this.Parent = caster.Entity;
            faction = caster.Entity.Faction;

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
            return Definition.ParseDescription();
        }
    }
}
