using Game.Agent;
using Game.Statistics;
using System;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [Serializable]
    public class SetStackBaseOnTechnologyLevel : ModifierBehaviour
    {
        [SerializeReference, SubclassSelector] private ModifierTarget target;
        [SerializeField] private StatisticReference divider;

        private StackModifierBehaviour stackModifierBehaviour;
        private AgentIdentity agentIdentity;

        public override void Initialize(ModifierEntity modifier)
        {
            base.Initialize(modifier);
            stackModifierBehaviour = modifier.Behaviours.OfType<StackModifierBehaviour>().FirstOrDefault();
            target.Initialize(modifier);
            divider.Initialize(modifier);
            agentIdentity = (target.GetTargets()[0] as Entity).GetCachedComponent<AgentIdentity>();
        }

        public override Result Update()
        {
            stackModifierBehaviour.SetStack((int)(agentIdentity.Agent.Technology.CurrentLevel / divider.Get()));
            return base.Update();
        }
    }
}
