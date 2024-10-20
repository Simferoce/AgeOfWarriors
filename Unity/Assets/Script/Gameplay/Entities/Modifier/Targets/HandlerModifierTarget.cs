using System;
using System.Collections.Generic;

namespace Game.Modifier
{
    [Serializable]
    public class HandlerModifierTarget : ModifierTarget
    {
        public override List<object> GetTargets()
        {
            return new List<object>() { modifier.Handler.Entity };
        }
    }
}
