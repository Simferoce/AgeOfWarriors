using System.Collections.Generic;

namespace AgeOfWarriors
{
    public class AgentCharacterFactory
    {
        private List<AgentFactoryCommand> commands = new List<AgentFactoryCommand>();
        private int slot = 1;

        public AgentCharacterFactory(Agent agent)
        {
        }

        public bool TryEnqueue(AgentFactoryCommand command)
        {
            if (commands.Count >= slot)
                return false;

            commands.Add(command);
            return true;
        }

        public void Update()
        {
            for (int i = commands.Count - 1; i >= 0; i--)
            {
                AgentFactoryCommand command = commands[i];

                if (command.IsFinish())
                {
                    command.Execute();
                    commands.Remove(command);
                }
            }
        }
    }
}