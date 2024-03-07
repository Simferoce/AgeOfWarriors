using Game;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class FactoryUI : MonoBehaviour
    {
        [SerializeField] private FactorySlotUI current;
        [SerializeField] private List<FactorySlotUI> factorySlotUIs = new List<FactorySlotUI>();

        public void Update()
        {
            Factory factory = Agent.Player.Factory;
            if (factory.Commands.Count > 0)
                factorySlotUIs[0].Refresh(factory.Commands[0].LaneObjectDefinition);
            else
                factorySlotUIs[0].Refresh(null);

            for (int i = 1; i < factorySlotUIs.Count; i++)
            {
                if (factory.Commands.Count > i)
                    factorySlotUIs[i].Refresh(factory.Commands[i].LaneObjectDefinition);
                else
                    factorySlotUIs[i].Refresh(null);
            }
        }
    }
}
