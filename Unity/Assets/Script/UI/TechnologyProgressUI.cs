using Game;
using UnityEngine;
using UnityEngine.UI;

public class TechnologyProgressUI : MonoBehaviour
{
    [SerializeField] private Image foreground;

    private void Update()
    {
        foreground.fillAmount = Agent.Player.Technology.CurrentTechnologyNormalized;
    }
}
