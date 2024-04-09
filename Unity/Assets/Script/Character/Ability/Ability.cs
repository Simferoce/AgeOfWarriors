using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class Ability : MonoBehaviour
    {
        public Character Character { get; set; }
        public AbilityDefinition Definition { get; set; }

        public bool IsCasting { get; set; }
        public virtual bool IsActive => IsCasting;
        public virtual List<IAttackable> Targets => new List<IAttackable>();

        [SerializeField] private StatisticHolder statistics;
        public StatisticHolder Statistics => statistics;

        private void OnValidate()
        {
            statistics.DefineReference("caster");
            statistics.DefineReference("base");
        }

        public virtual void Initialize(Character character)
        {
            this.Character = character;

            statistics.SetReference("caster", () => Character.Statistics);
            statistics.SetReference("base", () => Definition.Statistics);
            statistics.Initialize(this);
        }

        public abstract void Dispose();

        public abstract void Update();

        public abstract bool CanUse();

        public abstract void Use();

        public abstract void Interrupt();
    }
}
