using System;
using System.Collections.Generic;

namespace Game.Modifier
{
    [Serializable]
    public class ApplierModifierTarget : ModifierTarget
    {
        public override List<object> GetTargets()
        {
            return new List<object>() { modifier.Applier.Entity };
        }
    }
}
