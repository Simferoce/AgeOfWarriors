using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AbilityDefinition", menuName = "Definition/Ability")]
    public class AbilityDefinition : Definition
    {
        [SerializeField] private string title;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject prefab;

        [SerializeField] private StatisticHolder statistics;
        [StatisticResolve("statistics")] public StatisticHolder Statistics => statistics;

        public Sprite Icon { get => icon; set => icon = value; }
        public string Title { get => title; set => title = value; }

        public Ability GetAbility()
        {
            Ability clone = GameObject.Instantiate(prefab).GetComponent<Ability>();
            clone.Definition = this;
            return clone;
        }
    }
}
