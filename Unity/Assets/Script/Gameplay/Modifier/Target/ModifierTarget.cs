using System;
using System.Collections.Generic;

namespace Game
{
    [Serializable]
    public abstract class ModifierTarget
    {
        protected Modifier modifier;

        public void Initialize(Modifier modifier)
        {
            this.modifier = modifier;
        }

        public abstract List<object> GetTargets();
    }
}
