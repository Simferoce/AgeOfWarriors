using Game.Agent;
using TMPro;
using UnityEngine;

namespace Game.UI.Windows
{
    public class CurrencyUIElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currencyText;

        private void Update()
        {
            AgentEntity agentEntity = AgentRepository.Instance.GetByFaction(FactionType.Player);
            currencyText.text = agentEntity.Currency.ToString("0");
        }
    }
}