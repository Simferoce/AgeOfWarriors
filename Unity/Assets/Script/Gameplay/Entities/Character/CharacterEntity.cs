﻿using Game.Agent;
using Game.Components;
using Game.Modifier;
using Game.Statistics;
using Game.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Character
{
    [RequireComponent(typeof(Blocker))]
    [RequireComponent(typeof(Attackable))]
    [RequireComponent(typeof(Target))]
    [RequireComponent(typeof(AttackFactory))]
    [RequireComponent(typeof(ModifierApplier))]
    public partial class CharacterEntity : Entity<CharacterDefinition>, IHealable
    {
        [Header("Collision")]
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private Collider2D hitbox;

        public Animated Animated { get; set; }
        public List<TransformTag> TransformTags { get; set; }
        public Collider2D Hitbox { get => hitbox; set => hitbox = value; }

        public float Health { get => this[StatisticDefinitionRegistry.Instance.Health]; set => this[StatisticDefinitionRegistry.Instance.Health].Set(value); }
        public float MaxHealth => this[StatisticDefinitionRegistry.Instance.MaxHealth];
        public float Defense => this[StatisticDefinitionRegistry.Instance.Defense];
        public float AttackSpeed => this[StatisticDefinitionRegistry.Instance.AttackSpeed];
        public float AttackPower => this[StatisticDefinitionRegistry.Instance.AttackPower];
        public float Speed => this[StatisticDefinitionRegistry.Instance.Speed];
        public float Reach => this[StatisticDefinitionRegistry.Instance.Reach];
        public float TechnologyGainPerSecond => this[StatisticDefinitionRegistry.Instance.TechnologyGain];

        public bool IsEngaged => Time.time - this.GetCachedComponent<Attackable>().LastTimeAttacked < 2f || Time.time - this.GetCachedComponent<AttackFactory>().LastTimeAttackLanded < 2f;
        public bool IsInvulnerable => false;/*this.GetCachedComponent<GameModifierHandler>().GetModifiers().Any(x => x is IModifierInvulnerable);*/
        public bool IsConfused => false;/*this.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is ConfusionModifierDefinition.Modifier);*/
        public bool IsStaggered => this[StatisticDefinitionRegistry.Instance.Stagger];
        public bool IsDead => this.stateMachine.Current is DeathState;
        public bool IsInjured => this.Health < this.MaxHealth;
        public bool IsWeak => this[StatisticDefinitionRegistry.Instance.Weak];
        public bool IsRanged => this.Tags.Contains(EntityTag.Ranged);

        private StateMachine stateMachine = new StateMachine();
        private ModifierHandler modifierHandler;
        private AgentEntity agent;

        protected override void Awake()
        {
            base.Awake();
            TransformTags = GetComponentsInChildren<TransformTag>().ToList();
            GetCachedComponent<Attackable>().OnDamageTaken += OnDamageTaken;
            GetCachedComponent<AttackFactory>().OnAttackLanded += OnAttackLanded;
            Animated = GetComponentInChildren<Animated>();
            modifierHandler = GetCachedComponent<ModifierHandler>();
        }

        private void OnDamageTaken(AttackResult attackResult, Attackable attackable)
        {
            Health -= attackResult.DamageTaken;
            if (Health <= 0 && !IsDead)
                Death();
        }

        public override void Initialize()
        {
            base.Initialize();

            agent = GetCachedComponent<AgentIdentity>().Agent;

            StatisticRepository.Get("max_health").SetEquation((float baseValue) => baseValue + this[StatisticDefinitionRegistry.Instance.FlatMaxHealth]);
            StatisticRepository.Add(new StatisticFloat("flat_max_health", StatisticDefinitionRegistry.Instance.FlatMaxHealth, 0f, (float baseValue) => baseValue + modifierHandler[StatisticDefinitionRegistry.Instance.FlatMaxHealth]));
            StatisticRepository.Add(new StatisticFloat("health", StatisticDefinitionRegistry.Instance.Health, this[StatisticDefinitionRegistry.Instance.MaxHealth]));
            StatisticRepository.Get("defense").SetEquation((float baseValue) => baseValue + this[StatisticDefinitionRegistry.Instance.FlatDefense]);
            StatisticRepository.Get("attack_power").SetEquation((float baseValue) => baseValue + this[StatisticDefinitionRegistry.Instance.FlatAttackPower]);
            StatisticRepository.Get("attack_speed").SetEquation((float baseValue) => baseValue * (1 + this[StatisticDefinitionRegistry.Instance.PercentageAttackSpeed]) * this[StatisticDefinitionRegistry.Instance.MultiplierAttackSpeed]);
            StatisticRepository.Add(new StatisticFloat("percentage_attack_speed", StatisticDefinitionRegistry.Instance.PercentageAttackSpeed, 0f, (float baseValue) => baseValue + modifierHandler[StatisticDefinitionRegistry.Instance.PercentageAttackSpeed]));
            StatisticRepository.Add(new StatisticFloat("multiplier_attack_speed", StatisticDefinitionRegistry.Instance.MultiplierAttackSpeed, 1f, (float baseValue) => baseValue * modifierHandler[StatisticDefinitionRegistry.Instance.MultiplierAttackSpeed] * agent[StatisticDefinitionRegistry.Instance.MultiplierAttackSpeed]));
            StatisticRepository.Get("speed").SetEquation((float baseValue) => baseValue * this[StatisticDefinitionRegistry.Instance.MultiplierSpeed]);
            StatisticRepository.Get("reach").SetEquation((float baseValue) => baseValue * this[StatisticDefinitionRegistry.Instance.MultiplierReach]);
            StatisticRepository.Add(new StatisticFloat("flat_defense", StatisticDefinitionRegistry.Instance.FlatDefense, 0f, (float baseValue) => baseValue + modifierHandler[StatisticDefinitionRegistry.Instance.FlatDefense]));
            StatisticRepository.Add(new StatisticBool("stagger", StatisticDefinitionRegistry.Instance.Stagger, false, (bool baseValue) => baseValue || modifierHandler[StatisticDefinitionRegistry.Instance.Stagger]));
            StatisticRepository.Add(new StatisticFloat("ranged_damage_taken", StatisticDefinitionRegistry.Instance.RangeDamageTaken, 1f, (float baseValue) => baseValue * modifierHandler[StatisticDefinitionRegistry.Instance.RangeDamageTaken]));
            StatisticRepository.Add(new StatisticFloat("damage_taken", StatisticDefinitionRegistry.Instance.DamageTaken, 1f, (float baseValue) => baseValue * modifierHandler[StatisticDefinitionRegistry.Instance.DamageTaken]));
            StatisticRepository.Add(new StatisticFloat("multiplier_damage", StatisticDefinitionRegistry.Instance.MultiplierDamage, 1f, (float baseValue) => baseValue * modifierHandler[StatisticDefinitionRegistry.Instance.MultiplierDamage]));
            StatisticRepository.Add(new StatisticFloat("flat_attack_power", StatisticDefinitionRegistry.Instance.FlatAttackPower, 0f, (float baseValue) => baseValue + modifierHandler[StatisticDefinitionRegistry.Instance.FlatAttackPower] + agent[StatisticDefinitionRegistry.Instance.FlatAttackPower]));
            StatisticRepository.Add(new StatisticFloat("flat_damage_versus_weak", StatisticDefinitionRegistry.Instance.FlatDamageVersusWeak, 0f, (float baseValue) => baseValue + modifierHandler[StatisticDefinitionRegistry.Instance.FlatDamageVersusWeak]));
            StatisticRepository.Add(new StatisticBool("weak", StatisticDefinitionRegistry.Instance.Weak, false, (bool baseValue) => baseValue || modifierHandler[StatisticDefinitionRegistry.Instance.Weak]));
            StatisticRepository.Add(new StatisticBool("dead", null, false, (bool baseValue) => baseValue || IsDead));
            StatisticRepository.Add(new StatisticFloat("multiplier_speed", StatisticDefinitionRegistry.Instance.MultiplierSpeed, 1f, (float baseValue) => baseValue * modifierHandler[StatisticDefinitionRegistry.Instance.MultiplierSpeed] * agent[StatisticDefinitionRegistry.Instance.MultiplierSpeed]));
            StatisticRepository.Add(new Statistic<FactionType>("faction", null, new SerializeValue<FactionType>(), (FactionType baseValue) => GetCachedComponent<AgentIdentity>().Faction));
            StatisticRepository.Add(new StatisticFloat("multiplier_reach", StatisticDefinitionRegistry.Instance.MultiplierReach, 1f, (float baseValue) => baseValue * modifierHandler[StatisticDefinitionRegistry.Instance.MultiplierReach] * agent[StatisticDefinitionRegistry.Instance.MultiplierReach] * (IsRanged ? (1 + this[StatisticDefinitionRegistry.Instance.PercentageMultiplierReach]) : 1f)));
            StatisticRepository.Add(new StatisticBool("invulnerable", StatisticDefinitionRegistry.Instance.Invulnerable, false, (bool baseValue) => baseValue || modifierHandler[StatisticDefinitionRegistry.Instance.Invulnerable]));
            StatisticRepository.Add(new StatisticBool("engaged", null, false, (bool baseValue) => baseValue || IsEngaged));
            StatisticRepository.Add(new StatisticFloat("thorn", StatisticDefinitionRegistry.Instance.Thorn, 0f, (float baseValue) => baseValue + modifierHandler[StatisticDefinitionRegistry.Instance.Thorn]));
            StatisticRepository.Add(new StatisticFloat("ranged_percentage_reach", StatisticDefinitionRegistry.Instance.PercentageMultiplierReach, 0f, (float baseValue) => baseValue + modifierHandler[StatisticDefinitionRegistry.Instance.PercentageMultiplierReach] + agent[StatisticDefinitionRegistry.Instance.PercentageMultiplierReach]));
            Health = MaxHealth;

            Caster caster = GetCachedComponent<Caster>();
            caster.Initialize();

            stateMachine.Initialize(new MoveState(this));
        }

        public void Update()
        {
            stateMachine.Update();
        }

        public void RefreshDirection()
        {
            //transform.localScale = new Vector3(Mathf.Sign(IsConfused ? -Direction : Direction) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        public void Death()
        {
            GetCachedComponent<Blocker>().enabled = false;
            GetCachedComponent<Target>().enabled = false;
            GetCachedComponent<Caster>().enabled = false;
            GetCachedComponent<Attackable>().OnDamageTaken -= OnDamageTaken;
            GetCachedComponent<AttackFactory>().OnAttackLanded -= OnAttackLanded;
            EventChannelHandler.Publish(new DeathEventChannel.Event() { Entity = this });

            stateMachine.SetState(new DeathState(this));
        }

        public void Displace(Vector2 displacement)
        {
            rigidbody.MovePosition(this.rigidbody.position + displacement);
        }

        public void Heal(float amount)
        {
            Health += amount;
            Health = Mathf.Min(Health, MaxHealth);
        }

        public void OnAttackLanded(AttackResult attackResult)
        {
            Heal(attackResult.DamageTaken * attackResult.AttackData.Leach);
        }

        public void BeginCast()
        {
            stateMachine.SetState(new CastingState(this));
        }

        public void EndCast()
        {
            if (stateMachine.Next == null)
                stateMachine.SetState(new MoveState(this));
        }
    }
}