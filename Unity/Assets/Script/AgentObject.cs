using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public abstract class AgentObject : MonoBehaviour, IModifiable, IAttackable, IBlocker, IAttackSource
    {
        public static List<AgentObject> All { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            All = new List<AgentObject>();
        }

        public int Direction { get; protected set; }
        public Agent Agent { get; protected set; }
        public int SpawnNumber { get; private set; }
        public virtual float TechnologyGainPerSecond { get => 0f; }
        public virtual bool IsActive { get => true; }
        public virtual int Priority { get => SpawnNumber; }
        public virtual Faction Faction { get => Agent.Faction; }
        public Vector3 Position { get => transform.position; }

        protected virtual void Awake()
        {
            All.Add(this);
        }

        protected virtual void OnDestroy()
        {
            All.Remove(this);

            OnDestroyModifiable();
        }

        public virtual AgentObjectDefinition GetDefinition() { return null; }

        public virtual void Spawn(Agent agent, int spawnNumber, int direction)
        {
            this.Direction = direction;
            this.SpawnNumber = spawnNumber;
            this.Agent = agent;

            if (direction < 0)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            AwakeAttackable();
        }

        #region Modifiable
        protected ModifierHandler modifierHandler = new ModifierHandler();

        public void AddModifier(Modifier modifier)
        {
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
        [SerializeField] private Collider2D hitbox;
        [SerializeField] private Transform targetPosition;

        public abstract float MaxHealth { get; }
        public abstract float Defense { get; }
        public float Health { get; set; }
        public virtual bool IsDead { get => this.Health <= 0; }
        public event Action<Attack, IAttackable> OnDamageTaken;

        public Vector3 TargetPosition => targetPosition.position;

        public void AwakeAttackable()
        {
            this.Health = MaxHealth;
        }

        public bool IsAttackable()
        {
            return this.Health > 0;
        }

        public void TakeAttack(Attack attack)
        {
            float damageReduced = DefenseFormulaDefinition.Instance.ParseDamage(attack.Damage, Mathf.Max(0, Defense - attack.ArmorPenetration));
            this.Health -= damageReduced;

            foreach (IAttackSource source in attack.AttackSource.Sources)
                source.AttackLanded(attack, damageReduced);

            Debug.Log($"{this.name} took {damageReduced} from {attack.AttackSource.Sources[^1]}");
            OnDamageTaken?.Invoke(attack, this);

            if (Health <= 0 && !IsDead)
                Death();
        }

        public Vector3 ClosestPoint(Vector3 point)
        {
            return hitbox.ClosestPoint(point);
        }

        public virtual void Death()
        {
            Destroy(this.gameObject);
        }

        public void Heal(float amount)
        {
            this.Health += amount;
            this.Health = Mathf.Clamp(Health, 0, MaxHealth);
        }

        public virtual void Stagger(float duration)
        {

        }

        #region AttackSource
        public void AttackLanded(Attack attack, float damageDealt)
        {
            Heal(damageDealt * attack.Leach);
        }
        #endregion

        #endregion

        #region Blocker
        [SerializeField] private List<Tag> blocking = new List<Tag>();

        public Collider2D Collider => hitbox;

        public bool IsBlocking(AgentObject agentObject)
        {
            bool match = MatchAny(blocking, agentObject);
            if (match && agentObject.Faction == this.Faction)
                return Priority < agentObject.Priority;

            return match;
        }
        #endregion

        #region Tag
        [SerializeField] private List<Tag> inherentTag = new List<Tag>();

        public bool MatchAll(List<Tag> tags, IAttackable target)
        {
            List<Tag> targetTags = this.EvaluateContextualTags(target);
            return tags.All(x => targetTags.Contains(x));
        }

        public bool MatchAny(List<Tag> tags, IAttackable target)
        {
            List<Tag> targetTags = this.EvaluateContextualTags(target);
            return tags.Any(x => targetTags.Contains(x));
        }

        public List<Tag> EvaluateContextualTags(IAttackable target)
        {
            List<Tag> result = new List<Tag>(inherentTag);
            if (target.Faction != this.Agent.Faction)
                result.Add(Tag.Enemy);
            else
                result.Add(Tag.Ally);

            return result;
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