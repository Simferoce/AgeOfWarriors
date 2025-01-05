using UnityEngine;

namespace Game.Ability
{
    [CreateAssetMenu(fileName = "AbilityDefinition", menuName = "Definition/AbilityDefinition")]
    public class AbilityDefinition : Definition, ICooldown
    {
        [SerializeField] private string title;
        [SerializeField] private Sprite icon;
        [SerializeField] private Description description;
        [SerializeField] private AbilityEntity prefab;

        public string Title { get => title; }
        public Sprite Icon { get => icon; set => icon = value; }
        public float Remaining => prefab.Remaining;
        public float TotalCooldown => prefab.TotalCooldown;

        public virtual string ParseDescription(Entity source)
        {
            if (source == null)
                return description.Parse(prefab.GetComponent<Entity>(), false);

            return description.Parse(source, true);
        }

        public AbilityEntity GetAbility()
        {
            AbilityEntity ability = Instantiate(prefab);
            ability.Definition = this;
            return ability;
        }
    }
}
