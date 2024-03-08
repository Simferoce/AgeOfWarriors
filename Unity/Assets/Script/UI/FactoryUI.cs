using Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FactoryUI : MonoBehaviour
    {
        [SerializeField] private FactorySlotUI current;
        [SerializeField] private List<FactorySlotUI> factorySlotUIs = new List<FactorySlotUI>();
        [SerializeField] private Image progressBar;

        public void Update()
        {
            Factory factory = Agent.Player.Factory;
            if (factory.Commands.Count > 0)
                current.Refresh(factory.Commands[0].LaneObjectDefinition);
            else
                current.Refresh(null);

            if (factory.Commands.Count > 0)
            {
                progressBar.fillAmount = factory.TimeBeforeNextProductionNormalized;
            }
            else
            {
                progressBar.fillAmount = 0;
            }

            for (int i = 0; i < factorySlotUIs.Count; i++)
            {
                if (factory.Commands.Count > i + 1)
                    factorySlotUIs[i].Refresh(factory.Commands[i + 1].LaneObjectDefinition);
                else
                    factorySlotUIs[i].Refresh(null);
            }
        }
    }
}
