using Game.Components;
using Game.EventChannel;
using Game.Modifier;
using Game.Statistics;
using Game.Utilities;
using System;
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
    public partial class CharacterEntity : Entity<CharacterDefinition>
    {
        [Header("Collision")]
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private Collider2D hitbox;

        public event Action OnDeath;

        public Animated Animated { get; set; }
        public List<TransformTag> TransformTags { get; set; }
        public Collider2D Hitbox { get => hitbox; set => hitbox = value; }

        public float Health { get; set; }
        public float MaxHealth => modifierHandler.Modify(StatisticDefinitionRegistry.Instance.MaxHealth, definition.MaxHealth);
        public float Defense => modifierHandler.Modify(StatisticDefinitionRegistry.Instance.Defense, definition.Defense);
        public float AttackSpeed => modifierHandler.Modify(StatisticDefinitionRegistry.Instance.AttackSpeed, definition.AttackSpeed);
        public float AttackPower => modifierHandler.Modify(StatisticDefinitionRegistry.Instance.AttackPower, definition.AttackPower);
        public float Speed => modifierHandler.Modify(StatisticDefinitionRegistry.Instance.Speed, definition.Speed);
        public float Reach => modifierHandler.Modify(StatisticDefinitionRegistry.Instance.Reach, definition.Reach);
        public float TechnologyGainPerSecond => definition.TechnologyGainPerSecond;

        public bool IsEngaged => Time.time - this.GetCachedComponent<Attackable>().LastTimeAttacked < 1f || this.GetCachedComponent<AttackFactory>().LastTimeAttackLanded < 1f;
        public bool IsInvulnerable => false;/*this.GetCachedComponent<GameModifierHandler>().GetModifiers().Any(x => x is IModifierInvulnerable);*/
        public bool IsConfused => false;/*this.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is ConfusionModifierDefinition.Modifier);*/
        public bool IsStaggered => false;
        public bool IsDead => this.stateMachine.Current is DeathState;
        public bool IsInjured => this.Health < this.MaxHealth;

        private StateMachine stateMachine = new StateMachine();
        private ModifierHandler modifierHandler;

        protected override void Awake()
        {
            base.Awake();
            TransformTags = GetComponentsInChildren<TransformTag>().ToList();
            GetCachedComponent<Attackable>().OnDamageTaken += OnDamageTaken;
            GetCachedComponent<AttackFactory>().OnAttackLanded += OnAttackLanded;
            Animated = GetComponentInChildren<Animated>();
            modifierHandler = GetCachedComponent<ModifierHandler>();
        }

        public override bool TryGetStatistic<T>(StatisticDefinition definition, out T value)
        {
            if (definition == StatisticDefinitionRegistry.Instance.AttackPower)
            {
                value = StatisticConverter.ConvertGeneric<T, float>(AttackPower);
                return true;
            }
            else if (definition == StatisticDefinitionRegistry.Instance.Health)
            {
                value = StatisticConverter.ConvertGeneric<T, float>(Health);
                return true;
            }
            else if (definition == StatisticDefinitionRegistry.Instance.MaxHealth)
            {
                value = StatisticConverter.ConvertGeneric<T, float>(MaxHealth);
                return true;
            }
            else if (definition == StatisticDefinitionRegistry.Instance.Speed)
            {
                value = StatisticConverter.ConvertGeneric<T, float>(Speed);
                return true;
            }
            else if (definition == StatisticDefinitionRegistry.Instance.Reach)
            {
                value = StatisticConverter.ConvertGeneric<T, float>(Reach);
                return true;
            }
            else if (definition == StatisticDefinitionRegistry.Instance.AttackSpeed)
            {
                value = StatisticConverter.ConvertGeneric<T, float>(AttackSpeed);
                return true;
            }
            else if (definition == StatisticDefinitionRegistry.Instance.Defense)
            {
                value = StatisticConverter.ConvertGeneric<T, float>(Defense);
                return true;
            }

            return base.TryGetStatistic(definition, out value);
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

            Health = MaxHealth;
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
            DeathEventChannel.Instance.Publish(new DeathEventChannel.Event() { Entity = this });

            OnDeath?.Invoke();
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