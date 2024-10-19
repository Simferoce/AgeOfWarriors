using System;
using UnityEngine;

namespace Game.Agent
{
    [Serializable]
    public class AgentBehaviourAI : AgentBehaviour
    {
        [SerializeField] private bool active = true;

        private int next;

        public override void Initialize(AgentEntity agent)
        {
            base.Initialize(agent);
            this.agent = agent;
            next = ChooseNextToSpawn();
        }

        public override void OnLevelUp()
        {

        }

        public override void Update()
        {
            if (active == false)
                return;

            if (agent.TryQueueSpawnAgentObject(next))
            {
                next = ChooseNextToSpawn();
            }
        }

        private int ChooseNextToSpawn()
        {
            return UnityEngine.Random.Range(0, agent.Loadout.AmountOfAgentObjectAvailable);
        }
    }
}
