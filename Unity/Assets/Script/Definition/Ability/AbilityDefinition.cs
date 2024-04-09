using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AbilityDefinition", menuName = "Definition/Ability")]
    public class AbilityDefinition : Definition
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeReference, SerializeReferenceDropdown] private List<Statistic> statistics;
        [SerializeField] private GameObject prefab;

        public Sprite Icon { get => icon; set => icon = value; }
        public string Title { get => title; set => title = value; }

        public Statistic GetStatistic(string path)
        {
            Statistic statistic = statistics.FirstOrDefault(x => x.Name.ToLower() == path);

            Debug.Assert(statistic != null, $"Could not find path: {path}");

            return statistic as Statistic;
        }

        public string ParseDescription(object caller)
        {
            string description = this.description;
            foreach (Statistic statistic in statistics)
            {
                description = description.Replace($"{{{statistic.Name.ToLower()}}}", $"{statistic.GetValueText(caller)} ({statistic.GetDescription(caller)})");
            }

            return description;
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
