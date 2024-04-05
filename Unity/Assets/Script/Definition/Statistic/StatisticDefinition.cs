using UnityEngine;

namespace Game
{
    public abstract class StatisticDefinition : Definition
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string title;

        public Sprite Icon { get => icon; }
        public string Title { get => title; }

        public abstract string GetFormatedValue(AgentObject agentObject);
    }

    public abstract class StatisticDefinition<T> : StatisticDefinition
    {
        public override string GetFormatedValue(AgentObject agentObject)
        {
            return GetValue(agentObject).ToString();
        }

        public abstract T GetValue(AgentObject agentObject);
    }
}
