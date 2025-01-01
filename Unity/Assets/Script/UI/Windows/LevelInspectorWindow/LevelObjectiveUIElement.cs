using TMPro;
using UnityEngine;

namespace Game.UI.Windows
{
    public class LevelObjectiveUIElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;

        public void Refresh(LevelObjective levelObjectiveDefinition)
        {
            if (levelObjectiveDefinition == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                title.text = levelObjectiveDefinition.Title;
                description.text = levelObjectiveDefinition.Description;
            }
        }
    }
}
