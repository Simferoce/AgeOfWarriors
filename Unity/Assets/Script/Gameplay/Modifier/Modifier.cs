using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(AttackFactory))]
    public class Modifier : Entity
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
            this.Parent = handler.Entity;

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

        public override bool IsName(ReadOnlySpan<char> name)
        {
            return name.SequenceEqual("modifier") || base.IsName(name);
        }

        public override IEnumerable<Statistic> GetStatistic()
        {
            foreach (Statistic statistic in statistics)
                yield return statistic;

            foreach (Statistic statistic in base.GetStatistic())
                yield return statistic;
        }
    }
}
