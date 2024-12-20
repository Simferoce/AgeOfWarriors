﻿using System;
using System.Collections.Generic;

namespace Game.Modifier
{
    [Serializable]
    public abstract class ModifierTarget
    {
        protected ModifierEntity modifier;

        public virtual void Initialize(ModifierEntity modifier)
        {
            this.modifier = modifier;
        }

        public abstract List<object> GetTargets();
    }
}
