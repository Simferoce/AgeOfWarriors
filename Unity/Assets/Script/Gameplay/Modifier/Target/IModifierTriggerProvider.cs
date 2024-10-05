using System.Collections.Generic;

namespace Game
{
    public interface IModifierTriggerProvider
    {
        public List<ModifierTrigger> GetModifierTriggers();
    }
}
