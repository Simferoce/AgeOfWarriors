﻿using Game.UI.Windows;
using System;
using System.Linq;

namespace Game.Agent
{
    [Serializable]
    public class AgentBehaviourPlayer : AgentBehaviour
    {
        public override void OnLevelUp()
        {
            TechnologyWindow.Open(Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.GetCachedComponent<AgentIdentity>().Faction == FactionType.Player));
        }
    }
}