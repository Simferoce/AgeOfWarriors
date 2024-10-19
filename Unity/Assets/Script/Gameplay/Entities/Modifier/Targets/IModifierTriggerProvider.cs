using System.Collections.Generic;

namespace Game.Modifier
{
    public interface IModifierTriggerProvider
    {
        public List<ModifierTrigger> GetModifierTriggers();
    }
}
