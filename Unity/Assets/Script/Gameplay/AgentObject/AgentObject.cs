using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ModifierHandler))]
    public abstract class AgentObject : CachedMonobehaviour, IStatisticProvider
    {
        public delegate void AttackedLanded(AttackResult attack);

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

        public event System.Action<AgentObject> OnDestroyed;

        public virtual bool IsActive { get => true; }
        public int Direction { get; protected set; }
        public Agent Agent { get; protected set; }
        public int SpawnNumber { get; private set; }
        public int Priority { get => SpawnNumber; }
        public virtual Faction Faction { get => Agent.Faction; }
        public virtual Faction OriginalFaction { get => Agent.Faction; }
        public List<Type> Types { get => types; }
        public string Name => name;
        public virtual string StatisticProviderName => "agentobject";

        private List<IStatisticProvider> statisticProviderChildren;

        protected virtual void Awake()
        {
            All.Add(this);
            statisticProviderChildren = GetComponentsInChildren<IStatisticProvider>().Where(x => x != (IStatisticProvider)this).ToList();
        }

        protected virtual void OnDestroy()
        {
            All.Remove(this);
            OnDestroyed?.Invoke(this);
        }

        public T GetStatisticOrDefault<T>(string path)
        {
            return TryGetStatistic<T>(path, out T statistic) ? statistic : default;
        }

        public T GetStatisticOrDefault<T>(string path, T defaultValue)
        {
            return TryGetStatistic<T>(path, out T statistic) ? statistic : defaultValue;
        }

        public virtual bool TryGetStatistic<T>(ReadOnlySpan<char> path, out T statistic)
        {
            switch (path)
            {
                default:
                    return TryGetStatisticInChildren(path, out statistic);
            }
        }

        private bool TryGetStatisticInChildren<T>(ReadOnlySpan<char> path, out T statistic)
        {
            foreach (IStatisticProvider statisticProviderChild in statisticProviderChildren)
            {
                if (statisticProviderChild.TryGetStatistic<T>(path, out statistic))
                    return true;
            }

            statistic = default;
            return false;
        }

        public virtual void Spawn(Agent agent, int spawnNumber, int direction)
        {
            this.Direction = direction;
            this.SpawnNumber = spawnNumber;
            this.Agent = agent;

            transform.localScale = new Vector3(Mathf.Sign(direction) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        public virtual AgentObjectDefinition GetDefinition() { return null; }
    }

    public abstract class AgentObject<T> : AgentObject
        where T : AgentObjectDefinition
    {
        public T Definition { get; set; }

        public override AgentObjectDefinition GetDefinition()
        {
            return Definition;
        }
    }
}