using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "CommanderActiveDefinition", menuName = "Definition/CommanderActiveDefinition")]
    public class CommanderActiveDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private Description description;
        [SerializeField] private GameObject prefab;

        public string ParseDescription(Entity source)
        {
            return description.Parse(source);
        }
    }
}
