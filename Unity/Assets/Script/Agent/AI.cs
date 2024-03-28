using System;

namespace Game
{
    [Serializable]
    public class AI : AgentBehaviour
    {
        private int next;

        public override void Initialize(Agent agent)
        {
            base.Initialize(agent);
            next = ChooseNextToSpawn();
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
