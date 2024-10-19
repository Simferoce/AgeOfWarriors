using UnityEngine;

namespace Game.Ability
{
    [CreateAssetMenu(fileName = "AbilityDefinition", menuName = "Definition/AbilityDefinition")]
    public class AbilityDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private GameObject prefab;

        public string Title { get => title; }
        public string Description { get => description; set => description = value; }

        public virtual string ParseDescription()
        {
            return Description;
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
