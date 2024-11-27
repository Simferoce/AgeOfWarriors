using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game
{
    public class Caster : CachedMonobehaviour
    {
        private AgentObject agentObject;

        public event System.Action OnCastBegin;
        public event System.Action OnCastEnd;

        public float LastAbilityUsed { get; set; }
        public List<TransformTag> TransformTags { get; set; }
        public Agent Agent => agentObject.Agent;
        public Faction Faction => agentObject.Faction;
        public Vector3 CenterPosition => agentObject.transform.position;
        public int Direction => agentObject.Direction;
        public AgentObject AgentObject => agentObject;
        public bool IsCasting { get; set; }

        private void Awake()
        {
            agentObject = GetComponentInParent<AgentObject>();
            TransformTags = GetComponentsInChildren<TransformTag>().ToList();
        }

        [Header("Abilities")]
        [SerializeField] private List<AbilityDefinition> abilitiesDefinition = new List<AbilityDefinition>();

        public event Action<Ability> OnAbilityUsed;

        public List<Ability> Abilities { get => abilities; set => abilities = value; }

        private List<Ability> abilities = new List<Ability>();

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
            Profiler.BeginSample("Test");
            int v = AgentObject.GetStatisticOrDefault<int>("attackpower", 1);
            Profiler.EndSample();

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

        public bool CanUseAbility()
        {
            if (AgentObject.GetStatisticOrDefault("health", 0.0f) <= 0 || AgentObject.GetStatisticOrDefault("isDead", false))
                return false;

            return true;
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