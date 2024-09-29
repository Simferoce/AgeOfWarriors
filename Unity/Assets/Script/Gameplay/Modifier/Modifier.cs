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

        public delegate void OnRemovedDelegate(Modifier modifier);
        public event OnRemovedDelegate OnRemoved;

        public ModifierDefinition Definition { get; set; }
        public ModifierHandler Handler { get; set; }
        public ModifierApplier Applier { get; set; }
        public List<ModifierBehaviour> Behaviours { get => behaviours; set => behaviours = value; }
        public bool IsVisible { get => visibleByDefault; }

        public void Initialize(ModifierHandler handler, ModifierApplier applier)
        {
            this.Handler = handler;
            this.Applier = applier;
            this.Parent = handler.Entity;
            this.transform.parent = handler.transform;

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
            OnRemoved?.Invoke(this);

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
