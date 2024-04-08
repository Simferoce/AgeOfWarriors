using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class CharacterAbility
    {
        [StatisticResolve("caster")]
        public Character Character { get; set; }
        [StatisticResolve("base")]
        public AbilityDefinition Definition { get; set; }

        public bool IsCasting { get; set; }
        public virtual bool IsActive => IsCasting;
        public virtual List<IAttackable> Targets => new List<IAttackable>();

        [SerializeReference, SerializeReferenceDropdown] private IStatisticFloat range;
        [StatisticResolve("range")] public IStatisticFloat Range => range;

        public CharacterAbility()
        {

        }

        public CharacterAbility(CharacterAbility other)
        {
            range = other.range.Clone();
        }

        public virtual void Initialize(Character character)
        {
            this.Character = character;
        }

        public abstract void Dispose();

        public abstract void Update();

        public abstract bool CanUse();

        public abstract void Use();

        public abstract void Interrupt();

        public abstract CharacterAbility Clone();
    }
}
