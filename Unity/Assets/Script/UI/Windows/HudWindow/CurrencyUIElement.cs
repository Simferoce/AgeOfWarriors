using Game.Agent;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.UI.Windows
{
    public class CurrencyUIElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currencyText;

        private void Update()
        {
            AgentEntity agentEntity = Entity.All.OfType<AgentEntity>().FirstOrDefault(x => x.Faction == FactionType.Player);
            currencyText.text = agentEntity.Currency.ToString("0");
        }
    }
}