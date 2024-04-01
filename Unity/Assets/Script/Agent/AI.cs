using System;

namespace Game
{
    [Serializable]
    public class AI
    {
        private Agent agent;

        private int next;

        public void Initialize(Agent agent)
        {
            this.agent = agent;
            next = ChooseNextToSpawn();
        }

        public void Update()
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
