using System;

namespace Game
{
    [Serializable]
    public abstract class Requirement
    {
        public abstract bool Execute(Agent agent);

        public abstract string Format(Agent agent);
    }
}
