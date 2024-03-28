using Game;
using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;

    private void Update()
    {
        currencyText.text = Agent.Player.Currency.ToString("0");
    }
}
