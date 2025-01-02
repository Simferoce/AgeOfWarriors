using UnityEngine;

namespace Game.Ability
{
    [CreateAssetMenu(fileName = "AbilityDefinition", menuName = "Definition/AbilityDefinition")]
    public class AbilityDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private Sprite icon;
        [SerializeField] private Description description;
        [SerializeField] private GameObject prefab;

        public string Title { get => title; }
        public Sprite Icon { get => icon; set => icon = value; }

        public virtual string ParseDescription(Entity source)
        {
            if (source == null)
                return description.Parse(prefab.GetComponent<Entity>());

            return description.Parse(source);
        }

        public AbilityEntity GetAbility()
        {
            GameObject gameObject = Instantiate(prefab);
            AbilityEntity ability = gameObject.GetComponent<AbilityEntity>();
            ability.Definition = this;
            return ability;
        }
    }
}
