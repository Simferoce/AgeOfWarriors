using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AIBehaviour : AgentBehaviour
    {
        [SerializeField] private bool active = true;

        private int next;

        public override void Initialize(Agent agent)
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

            if (agent.SpawnLaneObject(next))
            {
                next = ChooseNextToSpawn();
            }
        }

        private int ChooseNextToSpawn()
        {
            return UnityEngine.Random.Range(0, agent.Factory.AmountOfAgentObjectAvailable);
        }
    }
}
