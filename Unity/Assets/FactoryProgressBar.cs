using Game;
using UnityEngine;
using UnityEngine.UI;

public class FactoryProgressBar : MonoBehaviour
{
    [SerializeField]
    private Image bar;

    public void Update()
    {
        bar.fillAmount = Agent.Player.Factory.TimeBeforeNextProductionNormalized;
    }
}
