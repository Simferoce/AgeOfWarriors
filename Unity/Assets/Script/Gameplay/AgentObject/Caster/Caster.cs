using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public partial class Caster : MonoBehaviour, IComponent, IStatisticContext
    {
        [Header("Abilities")]
        [SerializeField] private List<AbilityDefinition> abilitiesDefinition = new List<AbilityDefinition>();

        public Entity Entity { get; set; }

        public event Action<Ability> OnAbilityUsed;
        public event Action OnCastBegin;
        public event Action OnCastEnd;

        public List<Ability> Abilities { get => abilities; set => abilities = value; }
        public float LastAbilityUsed { get; set; }
        public List<TransformTag> TransformTags { get; set; }
        public bool IsCasting { get; set; }

        private List<Ability> abilities = new List<Ability>();

        private void Awake()
        {
            TransformTags = GetComponentsInChildren<TransformTag>().ToList();
        }

        private void Start()
        {
            GameObject abilitiesParent = new GameObject("Abilities");
            abilitiesParent.transform.parent = transform;

            foreach (AbilityDefinition definition in abilitiesDefinition)
            {
                Ability ability = definition.GetAbility();
                ability.transform.parent = abilitiesParent.transform;
                ability.Initialize(this);
                ability.OnAbilityUsed += Ability_OnAbilityUsed;

                abilities.Add(ability);
            }
        }

        private void Ability_OnAbilityUsed(Ability ability)
        {
            OnAbilityUsed?.Invoke(ability);
        }

        private void Update()
        {
            foreach (Ability ability in abilities)
            {
                if (ability.IsActive)
                    ability.Tick();
            }
        }

        private void OnDestroy()
        {
            foreach (Ability ability in abilities)
            {
                ability.Dispose();
                ability.OnAbilityUsed -= Ability_OnAbilityUsed;
            }
        }

        public void Interupt()
        {
            foreach (Ability ability in abilities)
            {
                if (ability.IsActive)
                    ability.Interrupt();
            }
        }

        public Ability GetCurrentAbility()
        {
            return abilities.FirstOrDefault(a => a.IsActive);
        }

        public void BeginCast()
        {
            IsCasting = true;
            OnCastBegin?.Invoke();
        }

        public void EndCast()
        {
            IsCasting = false;
            OnCastEnd?.Invoke();
        }

        public bool IsName(ReadOnlySpan<char> name)
        {
            return name.SequenceEqual("caster");
        }

        public IEnumerable<Statistic> GetStatistic()
        {
            foreach (Statistic statistic in Entity.GetStatistic())
                yield return statistic;
        }
    }
}