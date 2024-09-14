using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace Game
{
    public class Caster : MonoBehaviour, IComponent
    {
        [Header("Abilities")]
        [SerializeField] private List<AbilityDefinition> abilitiesDefinition = new List<AbilityDefinition>();

        public event Action<Ability> OnAbilityUsed;
        public event System.Action OnCastBegin;
        public event System.Action OnCastEnd;

        public List<Ability> Abilities { get => abilities; set => abilities = value; }
        public Entity Entity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            Profiler.BeginSample("Test");
            int v = StatisticUtility.ResolveStatisticOrDefault<int>(Entity as IStatisticProvider, "attackpower", 1);
            Profiler.EndSample();
            Debug.Log(v);

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
            if (StatisticUtility.ResolveStatisticOrDefault(Entity as IStatisticProvider, "health", 0.0f) <= 0 || StatisticUtility.ResolveStatisticOrDefault(Entity as IStatisticProvider, "isDead", false))
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