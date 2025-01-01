using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CommanderDefinition", menuName = "Definition/CommanderDefinition")]
    public class CommanderDefinition : Definition
    {
        [SerializeField] private Sprite icon;

        public Sprite Icon { get => icon; set => icon = value; }
    }
}
