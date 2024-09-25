using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Modifier : MonoBehaviour, IStatisticContext
    {
        [SerializeField] private bool visibleByDefault = true;
        [SerializeReference, SubclassSelector] private List<Statistic> statistics;
        [SerializeReference, SubclassSelector] private List<ModifierBehaviour> behaviours;

        public ModifierDefinition Definition { get; set; }
        public ModifierHandler Handler { get; set; }
        public IModifierSource Source { get; set; }
        public List<ModifierBehaviour> Behaviours { get => behaviours; set => behaviours = value; }
        public bool IsVisible { get => visibleByDefault; }

        public void Initialize(ModifierHandler handler, IModifierSource source, ModifierDefinition definition)
        {
            this.Handler = handler;
            this.Source = source;
            this.Definition = definition;

            foreach (ModifierBehaviour modifierBehaviour in Behaviours)
                modifierBehaviour.Initialize();
        }

        private void Update()
        {
            foreach (ModifierBehaviour modifierBehaviour in Behaviours)
                modifierBehaviour.Update();
        }

        private void OnDestroy()
        {
            foreach (ModifierBehaviour modifierBehaviour in Behaviours)
                modifierBehaviour.Dispose();
        }

        public string ParseDescription()
        {
            return Definition.ParseDescription();
        }

        public bool IsName(ReadOnlySpan<char> name)
        {
            return name.SequenceEqual("modifier");
        }

        public IEnumerable<Statistic> GetStatistic()
        {
            foreach (Statistic statistic in statistics)
                yield return statistic;
        }
    }
}
