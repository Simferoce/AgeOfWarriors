using UnityEngine;

namespace Game.Ability
{
    [CreateAssetMenu(fileName = "AbilityDefinition", menuName = "Definition/AbilityDefinition")]
    public class AbilityDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private Description description;
        [SerializeField] private GameObject prefab;

        public string Title { get => title; }

        public virtual string ParseDescription(object context)
        {
            return description.Parse(this, context);
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
