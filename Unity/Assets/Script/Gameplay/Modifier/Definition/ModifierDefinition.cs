using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "ModifierDefinition", menuName = "Definition/Modifier/ModifierDefinition")]
    public class ModifierDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private Sprite icon;
        [SerializeField] private string description;
        [SerializeField] private GameObject prefab;
        [SerializeField] private bool showOnHealthBar = true;

        public Sprite Icon { get => icon; }
        public string Title { get => title; }
        public bool Show { get => showOnHealthBar; set => showOnHealthBar = value; }
        public string Description { get => description; set => description = value; }

        public string ParseDescription() { return description; }

        public Modifier Instantiate(ModifierHandler handler, IModifierSource source)
        {
            GameObject gameObject = Instantiate(prefab, handler.transform);
            Modifier modifier = gameObject.GetComponent<Modifier>();
            modifier.Initialize(handler, source, this);
            return modifier;
        }
    }
}