using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Game.UI.Windows
{
    public class CommanderDescriptionUIElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI active;
        [SerializeField] private TextMeshProUGUI perk;

        private List<CharacterSelectionUIElement> characterSelectionUIElement;

        private void Awake()
        {
            characterSelectionUIElement = GetComponentsInChildren<CharacterSelectionUIElement>(true).ToList();
        }

        public void Refresh(CommanderDefinition commanderDefinition)
        {
            title.text = commanderDefinition.Title;
            active.text = $"<b>Active:</b>: {commanderDefinition.Active.ParseDescription(null)}";
            perk.text = $"<b>Perk:</b>: {commanderDefinition.Perk.ParseDescription(null)}";

            characterSelectionUIElement[0].Refresh(commanderDefinition.CharacterDefinitions[0]);
            characterSelectionUIElement[1].Refresh(commanderDefinition.CharacterDefinitions[1]);
            characterSelectionUIElement[2].Refresh(commanderDefinition.CharacterDefinitions[2]);
            characterSelectionUIElement[3].Refresh(commanderDefinition.CharacterDefinitions[3]);
        }
    }
}
