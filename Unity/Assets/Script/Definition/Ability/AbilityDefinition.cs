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
        [SerializeReference] private List<Statistic> statistics;
        [SerializeField] private GameObject prefab;

        public Sprite Icon { get => icon; set => icon = value; }
        public string Title { get => title; set => title = value; }

        public bool TryGetValue<T>(object context, StatisticDefinition definition, out T value)
        {
            Statistic<T> statistic = statistics.FirstOrDefault(x => x.Definition == definition) as Statistic<T>;
            if (statistic == null)
            {
                value = default(T);
                return false;
            }
            else
            {
                value = statistic.GetValue(context);
                return true;
            }
        }

        public string ParseDescription(object caller)
        {
            string description = this.description;
            foreach (Statistic statistic in statistics)
            {
                description = description.Replace($"{{{statistic.Definition.Title.ToLower()}}}", $"{statistic.GetValueText(caller)} {statistic.GetDescription()}");
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
