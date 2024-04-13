using Assets.Script.Agent.Technology;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public abstract class AgentObject : MonoBehaviour, IModifiable, IAttackable, IAttackSource, ITargeteable, IHealable, IShieldable
    {
        public enum Type
        {
            Building
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            All = new List<AgentObject>();
        }

        public static List<AgentObject> All { get; private set; }

        [SerializeField] private List<Type> types = new List<Type>();
        [SerializeField] private Collider2D hitbox;
        [SerializeField] private Transform targetPosition;

        public int Direction { get; protected set; }
        public Agent Agent { get; protected set; }
        public int SpawnNumber { get; private set; }
        public virtual bool IsActive { get => true; }
        public virtual int Priority { get => SpawnNumber; }
        public virtual Faction Faction { get => Agent.Faction; }
        public Vector3 CenterPosition { get => transform.position; }
        public Vector3 TargetPosition { get => targetPosition.position; }
        public Collider2D Collider { get => hitbox; }
        public virtual List<Type> Types { get => types; }

        protected virtual void Awake()
        {
            All.Add(this);
        }

        protected virtual void OnDestroy()
        {
            All.Remove(this);

            OnDestroyModifiable();
        }

        public void Update()
        {
            UpdateShields();
            modifierHandler.Update();
        }

        public virtual AgentObjectDefinition GetDefinition() { return null; }

        public virtual void Spawn(Agent agent, int spawnNumber, int direction)
        {
            this.Direction = direction;
            this.SpawnNumber = spawnNumber;
            this.Agent = agent;

            if (direction < 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            AgentObjectDefinition agentObjectDefinition = GetDefinition();
            List<ITechnologyModify> modifiers = agent.Technology.PerksUnlocked.OfType<ITechnologyModify>().ToList();
            foreach (ITechnologyModify modifier in modifiers)
            {
                if (modifier.Affect(agentObjectDefinition))
                    this.AddModifier(modifier.GetModifier(this));
            }

            this.Health = MaxHealth;
        }

        public Vector3 ClosestPoint(Vector3 point)
        {
            return hitbox.ClosestPoint(point);
        }

        public void Heal(float amount)
        {
            this.Health += amount;
            this.Health = Mathf.Clamp(Health, 0, MaxHealth);
        }

        public void Death()
        {
            OnDestroyed?.Invoke(this);
            EventChannelDeath.Instance.Publish(new EventChannelDeath.Event() { AgentObject = this });
            InternalDeath();
        }

        protected virtual void InternalDeath()
        {
            Destroy(this.gameObject);
        }

        public void AttackLanded(Attack attack, float damageDealt, bool killingBlow)
        {
            Heal(damageDealt * attack.Leach);
            OnAttackLanded?.Invoke(attack, damageDealt, killingBlow);
        }

        #region Statistic
        public virtual float MaxHealth { get; }
        public virtual float Defense { get; }
        public virtual float AttackPower { get; }
        public virtual float Reach { get; }
        public virtual float Speed { get; }
        public virtual float AttackSpeed { get; }
        public virtual float TechnologyGainPerSecond { get => 0f; }
        public float Health { get; private set; }

        public virtual bool IsDead { get => this.Health <= 0; }
        public virtual bool IsInvulnerable { get => false; }
        public virtual bool IsEngaged { get => false; }
        public bool IsInjured { get => !IsDead && Health < MaxHealth; }
        public bool IsDisplaceable { get => this is IDisplaceable; }
        public bool IsAttackable { get => this.Health > 0; }

        public virtual bool TryGetStatisticValue<T>(StatisticDefinition statisticDefinition, StatisticType statisticType, out T value)
        {
            value = default;
            return false;
        }
        #endregion

        #region Modifiable
        protected ModifierHandler modifierHandler = new ModifierHandler();

        public void AddModifier(Modifier modifier)
        {
            modifier.Initialize();
            modifierHandler.Add(modifier);
        }

        public void RemoveModifier(Modifier modifier)
        {
            modifierHandler.Remove(modifier);
        }

        public List<Modifier> GetModifiers()
        {
            return modifierHandler.Modifiers;
        }

        private void OnDestroyModifiable()
        {
            modifierHandler.Dispose();
        }
        #endregion

        #region Attackable
        #region Shield
        public event IShieldable.ShieldBroken OnShieldBroken;
        public event Action<IShieldable> OnDestroyed;

        private List<Shield> shields = new List<Shield>();
        public List<Shield> Shields { get => shields; set => shields = value; }

        public void AddShield(Shield shield)
        {
            shields.Add(shield);
        }

        public void UpdateShields()
        {
            for (int i = shields.Count - 1; i >= 0; i--)
            {
                Shield shield = shields[i];
                if (shield.Update())
                {
                    shields.Remove(shield);
                    OnShieldBroken?.Invoke(shield);
                }
            }
        }
        #endregion

        public event Action<Attack, IAttackable> OnDamageTaken;
        public delegate void AttackedLanded(Attack attack, float damageDealt, bool killingBlow);
        public event AttackedLanded OnAttackLanded;

        public void TakeAttack(Attack attack)
        {
            if (IsDead)
                return;

            if (IsInvulnerable)
                return;

            float damageRemaining = DefenseFormulaDefinition.Instance.ParseDamage(attack.Damage, Mathf.Max(0, Defense - attack.ArmorPenetration));
            for (int i = shields.Count - 1; i >= 0; i--)
            {
                Shield shield = shields[i];
                if (!shield.Absorb(damageRemaining, out damageRemaining))
                {
                    OnShieldBroken?.Invoke(shield);
                    shields.RemoveAt(i);
                }
            }

            this.Health -= damageRemaining;

            if (this.Health <= 0)
            {
                ResistKillingBlowPerk.Modifier modifier = (ResistKillingBlowPerk.Modifier)GetModifiers().FirstOrDefault(x => x is ResistKillingBlowPerk.Modifier modifier && modifier.CanResistsKillingBlow());
                if (modifier != null)
                {
                    modifier.ResistKillingBlow();
                    this.Health = 0.001f;
                }
            }

            foreach (IAttackSource source in attack.AttackSource.Sources)
                source.AttackLanded(attack, damageRemaining, this.Health <= 0);

            Debug.Log($"{this.name} took {damageRemaining} (reduced by {attack.Damage - damageRemaining}) from {attack.AttackSource.Sources[^1]}");
            OnDamageTaken?.Invoke(attack, this);

            if (Health <= 0 && !IsDead)
                Death();
        }

        #endregion
    }

    public abstract class AgentObject<T> : AgentObject
        where T : AgentObjectDefinition
    {
        public override float TechnologyGainPerSecond => Definition.TechnologyGainPerSecond;

        public T Definition { get; set; }

        public override AgentObjectDefinition GetDefinition()
        {
            return Definition;
        }
    }
}