using Game;
using TMPro;
using UnityEngine;

public class TechnologyLevelUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    public void Refresh(Agent agent)
    {
        textMeshProUGUI.text = Agent.Player.Technology.CurrentLevel.ToString();
    }
}
