using Game.Agent;
using Game.Character;
using System;
using System.Linq;

namespace Game.Ability
{
    [Serializable]
    public class IsFirstAbilityCondition : AbilityCondition
    {
        public override bool Execute()
        {
            if (ability.Caster.Entity is not AgentObject agentObject)
                return true;

            int minPriority = EntityRepository.Instance.GetByType<CharacterEntity>().Where(x => x.Agent == agentObject.Agent && !x.IsDead).Min(x => x.Priority);
            return minPriority == agentObject.Priority;
        }
    }
}
