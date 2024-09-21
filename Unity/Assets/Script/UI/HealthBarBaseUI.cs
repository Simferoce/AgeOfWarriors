using Game;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBaseUI : MonoBehaviour
{
    [SerializeField] private Image foreground;
    [SerializeField] private Faction faction;

    private void Update()
    {
        Agent agent = Agent.agents.FirstOrDefault(x => x.Faction == faction);
        foreground.fillAmount = agent.Base.Health.GetValueOrThrow<float>(agent.Base) / agent.Base.Health.Max.GetValueOrThrow<float>(agent.Base);
    }
}
