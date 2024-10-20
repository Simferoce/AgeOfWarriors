using System.Linq;
using UnityEngine;

namespace Game.Modifier
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

        public virtual UniqueType GetUniqueType()
        {
            UniqueModifierBehaviour uniqueModifierBehaviour = prefab.GetComponent<ModifierEntity>().Behaviours.OfType<UniqueModifierBehaviour>().FirstOrDefault();
            return uniqueModifierBehaviour != null ? uniqueModifierBehaviour.Type : UniqueType.None;
        }

        public ModifierEntity Instantiate()
        {
            GameObject gameObject = Instantiate(prefab);
            ModifierEntity modifier = gameObject.GetComponent<ModifierEntity>();
            modifier.Definition = this;

            return modifier;
        }
    }
}