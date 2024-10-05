using System.Collections.Generic;

namespace Game
{
    public interface IModifierTargetProvider
    {
        public List<object> GetTargets();
    }
}