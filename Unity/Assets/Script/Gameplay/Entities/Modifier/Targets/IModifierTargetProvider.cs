using System.Collections.Generic;

namespace Game.Modifier
{
    public interface IModifierTargetProvider
    {
        public List<object> GetTargets();
    }
}