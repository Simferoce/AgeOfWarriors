using Game;
using TMPro;
using UnityEngine;

public class TechnologyLevel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    private void Update()
    {
        textMeshProUGUI.text = Agent.Player.Technology.CurrentLevel.ToString();
    }
}
