using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    [Serializable]
    public class ModifierTargetTrigger : ModifierTarget
    {
        public override List<object> GetTargets()
        {
            return modifier.Behaviours.OfType<IModifierTriggerProvider>()
                .SelectMany(x => x.GetModifierTriggers())
                .OfType<IModifierTargetProvider>()
                .SelectMany(x => x.GetTargets())
                .ToList();
        }
    }
}