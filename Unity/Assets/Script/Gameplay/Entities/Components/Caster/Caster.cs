using Game.Ability;
using Game.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Components
{
    public partial class Caster : MonoBehaviour
    {
        [Header("Abilities")]
        [SerializeField] private List<AbilityDefinition> abilitiesDefinition = new List<AbilityDefinition>();

        public Entity Entity { get; set; }

        public event Action<AbilityEntity> OnAbilityUsed;
        public event Action OnAbilityInitialized;
        public event Action OnCastBegin;
        public event Action OnCastEnd;

        public float LastAbilityUsed { get; set; }
        public List<TransformTag> TransformTags { get; set; }
        public bool IsCasting { get; set; }
        public int AbilityCount => abilities.Count;
        public List<AbilityEntity> Abilities { get => abilities; }

        private List<AbilityEntity> abilities = new List<AbilityEntity>();
        private GameObject abilitiesParent;

        public void Initialize()
        {
            TransformTags = GetComponentsInChildren<TransformTag>().ToList();
            Entity = GetComponentInParent<Entity>();
            abilitiesParent = new GameObject("Abilities");
            abilitiesParent.transform.parent = transform;

            foreach (AbilityDefinition definition in abilitiesDefinition)
                AddAbility(definition);

            OnAbilityInitialized?.Invoke();
        }

        public void AddAbility(AbilityDefinition definition)
        {
            AbilityEntity ability = definition.GetAbility();
            ability.transform.parent = abilitiesParent.transform;
            ability.Initialize(this);

            abilities.Add(ability);
        }

        public bool CanUse(int index)
        {
            if (IsCasting)
                return false;

            return index < abilities.Count && abilities[index].CanUse();
        }

        public void Use(int index)
        {
            abilities[index].Use();
            OnAbilityUsed?.Invoke(abilities[index]);
        }

        public AbilityEntity Get(int index)
        {
            return abilities[index];
        }

        private void Update()
        {
            foreach (AbilityEntity ability in abilities)
            {
                if (ability.IsCasting)
                {
                    ability.Tick();
                }
            }
        }

        private void OnDestroy()
        {
            foreach (AbilityEntity ability in abilities)
                ability.Dispose();
        }

        public void Interupt()
        {
            foreach (AbilityEntity ability in abilities)
            {
                if (ability.IsCasting)
                    ability.Interrupt();
            }
        }

        public AbilityEntity GetCurrentAbility()
        {
            return abilities.FirstOrDefault(a => a.IsCasting);
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
    }
}