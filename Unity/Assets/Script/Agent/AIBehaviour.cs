using System;

namespace Game
{
    [Serializable]
    public class AIBehaviour : AgentBehaviour
    {
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
