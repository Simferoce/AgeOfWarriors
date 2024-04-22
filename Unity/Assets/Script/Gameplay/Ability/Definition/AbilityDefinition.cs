using UnityEngine;

namespace Game
{
    public abstract class AbilityDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private GameObject prefab;

        public string Title { get => title; }

        public virtual string ParseDescription()
        {
            return "";
        }

        public Ability GetAbility()
        {
            GameObject gameObject = Instantiate(prefab);
            Ability ability = gameObject.GetComponent<Ability>();
            ability.Definition = this;
            return ability;
        }
    }
}
