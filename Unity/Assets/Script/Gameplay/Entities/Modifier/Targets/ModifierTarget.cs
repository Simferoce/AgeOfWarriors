using System;
using System.Collections.Generic;

namespace Game.Modifier
{
    [Serializable]
    public abstract class ModifierTarget
    {
        protected ModifierEntity modifier;

        public void Initialize(ModifierEntity modifier)
        {
            this.modifier = modifier;
        }

        public abstract List<object> GetTargets();
    }
}
