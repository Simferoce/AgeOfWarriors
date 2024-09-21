using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "AbilityDefinition", menuName = "Definition/AbilityDefinition")]
    public class AbilityDefinition : Definition, IStatisticContext
    {
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private GameObject prefab;

        [SerializeReference, SubclassSelector] private List<Statistic> statistics = new List<Statistic>();

        public string Title { get => title; }
        public string Description { get => description; set => description = value; }

        public virtual string ParseDescription()
        {
            return Description;
        }

        public Ability GetAbility()
        {
            GameObject gameObject = Instantiate(prefab);
            Ability ability = gameObject.GetComponent<Ability>();
            ability.Definition = this;
            return ability;
        }

        public virtual bool IsName(ReadOnlySpan<char> name)
        {
            return name.SequenceEqual("definition");
        }

        public virtual Statistic GetStatistic(ReadOnlySpan<char> value)
        {
            foreach (Statistic statistic in statistics)
            {
                if (value.SequenceEqual(statistic.GetName(this)))
                    return statistic;
            }

            return null;
        }

        public virtual IStatisticContext GetContext(ReadOnlySpan<char> value)
        {
            return null;
        }
    }
}
