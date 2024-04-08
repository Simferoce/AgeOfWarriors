using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class CharacterAbility
    {
        public Character Character { get; set; }
        public AbilityDefinition Definition { get; set; }

        public bool IsCasting { get; set; }
        public virtual bool IsActive => IsCasting;
        public virtual List<IAttackable> Targets => new List<IAttackable>();

        //ability.base.range
        [SerializeReference, SubclassSelector] private IStatisticFloat range;
        public IStatisticFloat Range => range;

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
