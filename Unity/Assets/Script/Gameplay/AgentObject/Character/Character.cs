using Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Blocker))]
    [RequireComponent(typeof(Attackable))]
    [RequireComponent(typeof(Target))]
    [RequireComponent(typeof(AttackFactory))]
    [RequireComponent(typeof(ModifierApplier))]
    public partial class Character : AgentObject<CharacterDefinition>
    {
        [Header("Collision")]
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private Collider2D hitbox;

        public event Action OnDeath;

        public Animated Animated { get; set; }
        public List<TransformTag> TransformTags { get; set; }
        public override bool IsActive { get => !IsDead; }
        public Collider2D Hitbox { get => hitbox; set => hitbox = value; }
        public override Faction Faction => IsConfused ? Agent.Faction.GetConfusedFaction() : Agent.Faction;

        public float Health { get; set; }
        public float MaxHealth => Definition.MaxHealth + this.GetCachedComponent<StatisticIndex>().SumByDefinition(StatisticRepository.MaxHealthFlat);
        public float Defense => Definition.Defense /*+ this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.Defense.HasValue).Sum(x => x.Defense.Value)*/;
        public float AttackSpeed => Definition.AttackSpeed /** (1 + this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.AttackSpeedPercentage.HasValue).Sum(x => x.AttackSpeedPercentage.Value))*/;
        public float AttackPower => Definition.AttackPower /*+ this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.AttackPower.HasValue).Sum(x => x.AttackPower.Value)*/;
        public float Speed => Definition.Speed/* * (1 + this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.SpeedPercentage.HasValue).Sum(x => x.SpeedPercentage.Value))*/;
        public float Reach => Definition.Reach /** (1 + this.GetCachedComponent<ModifierHandler>().GetModifiers().Where(x => x.ReachPercentage.HasValue).Sum(x => x.ReachPercentage.Value))*/;
        public float TechnologyGainPerSecond => Definition.TechnologyGainPerSecond;

        public bool IsEngaged => Time.time - this.GetCachedComponent<Attackable>().LastTimeAttacked < 1f || this.GetCachedComponent<AttackFactory>().LastTimeAttackLanded < 1f;
        public bool IsInvulnerable => this.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is IInvulnerableModifier);
        public bool IsConfused => false;/*this.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is ConfusionModifierDefinition.Modifier);*/
        public bool IsStaggered => false;/*this.GetCachedComponent<ModifierHandler>().GetModifiers().Any(x => x is StaggerModifierDefinition.Modifier);*/
        public bool IsDead => this.stateMachine.Current is DeathState;
        public bool IsInjured => this.Health < this.MaxHealth;

        private StateMachine stateMachine = new StateMachine();

        protected override void Awake()
        {
            base.Awake();
            TransformTags = GetComponentsInChildren<TransformTag>().ToList();
            GetCachedComponent<Attackable>().OnDamageTaken += OnDamageTaken;
            GetCachedComponent<AttackFactory>().OnAttackLanded += OnAttackLanded;
            Animated = GetComponentInChildren<Animated>();
        }

        private void OnDamageTaken(AttackResult attackResult, Attackable attackable)
        {
            Health -= attackResult.DamageTaken;
            if (Health <= 0 && !IsDead)
                Death();
        }

        public override void Spawn(Agent agent, int spawnNumber, int direction)
        {
            base.Spawn(agent, spawnNumber, direction);

            StatisticIndex statisticIndex = GetCachedComponent<StatisticIndex>();
            statisticIndex.Add(new StatisticFunction<float>(() => Health, StatisticRepository.Health));
            statisticIndex.Add(new StatisticFunction<float>(() => MaxHealth, StatisticRepository.MaxHealth));
            statisticIndex.Add(new StatisticFunction<float>(() => AttackPower, StatisticRepository.AttackPower));
            statisticIndex.Add(new StatisticFunction<float>(() => AttackSpeed, StatisticRepository.AttackSpeed));
            statisticIndex.Add(new StatisticFunction<float>(() => Defense, StatisticRepository.Defense));
            statisticIndex.Add(new StatisticFunction<float>(() => Speed, StatisticRepository.Speed));
            statisticIndex.Add(new StatisticFunction<float>(() => Reach, StatisticRepository.Reach));

            Health = MaxHealth;

            stateMachine.Initialize(new MoveState(this));
        }

        public void Update()
        {
            if (IsDead)
                return;

            stateMachine.Update();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetCachedComponent<Attackable>().OnDamageTaken -= OnDamageTaken;
            GetCachedComponent<AttackFactory>().OnAttackLanded -= OnAttackLanded;
        }

        public void RefreshDirection()
        {
            transform.localScale = new Vector3(Mathf.Sign(IsConfused ? -Direction : Direction) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        public void Death()
        {
            EventChannelDeath.Instance.Publish(new EventChannelDeath.Event() { AgentObject = this });
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
        }

        public void OnAttackLanded(AttackResult attackResult)
        {
            Heal(attackResult.DamageTaken * attackResult.Attack.Leach);
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