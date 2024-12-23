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

        public List<AbilityEntity> Abilities { get => abilities; set => abilities = value; }
        public float LastAbilityUsed { get; set; }
        public List<TransformTag> TransformTags { get; set; }
        public bool IsCasting { get; set; }


        private List<AbilityEntity> abilities = new List<AbilityEntity>();

        private void Awake()
        {
            TransformTags = GetComponentsInChildren<TransformTag>().ToList();
            Entity = GetComponentInParent<Entity>();
        }

        private void Start()
        {
            GameObject abilitiesParent = new GameObject("Abilities");
            abilitiesParent.transform.parent = transform;

            foreach (AbilityDefinition definition in abilitiesDefinition)
            {
                AbilityEntity ability = definition.GetAbility();
                ability.transform.parent = abilitiesParent.transform;
                ability.Initialize(this);
                ability.OnAbilityUsed += Ability_OnAbilityUsed;

                abilities.Add(ability);
            }

            OnAbilityInitialized?.Invoke();
        }

        private void Ability_OnAbilityUsed(AbilityEntity ability)
        {
            OnAbilityUsed?.Invoke(ability);
        }

        private void Update()
        {
            foreach (AbilityEntity ability in abilities)
            {
                if (ability.IsCasting)
                    ability.Tick();
            }
        }

        private void OnDestroy()
        {
            foreach (AbilityEntity ability in abilities)
            {
                ability.Dispose();
                ability.OnAbilityUsed -= Ability_OnAbilityUsed;
            }
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