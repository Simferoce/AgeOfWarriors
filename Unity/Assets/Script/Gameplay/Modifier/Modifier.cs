using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(AttackFactory))]
    public class Modifier : Entity
    {
        [SerializeField] private bool visibleByDefault = true;
        [SerializeReference, SubclassSelector] private List<ModifierBehaviour> behaviours;

        public delegate void OnRemovedDelegate(Modifier modifier);
        public event OnRemovedDelegate OnRemoved;

        public ModifierDefinition Definition { get; set; }
        public ModifierHandler Handler { get; set; }
        public ModifierApplier Applier { get; set; }
        public List<ModifierBehaviour> Behaviours { get => behaviours; set => behaviours = value; }
        public bool IsVisible { get => visibleByDefault; }
        public List<ModifierParameter> Parameters { get => parameters; set => parameters = value; }

        private List<ModifierParameter> parameters;

        public void Initialize(ModifierHandler handler, ModifierApplier applier, params ModifierParameter[] parameters)
        {
            this.parameters = parameters.ToList();
            this.Handler = handler;
            this.Applier = applier;
            this.Parent = handler.Entity;
            this.transform.parent = handler.transform;

            foreach (ModifierBehaviour modifierBehaviour in Behaviours)
                modifierBehaviour.Initialize(this);
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

        public override IEnumerable<Statistic> GetStatistic()
        {
            foreach (Statistic statistic in behaviours.OfType<IStatisticContext>().SelectMany(x => x.GetStatistic()))
                yield return statistic;

            foreach (Statistic statistic in base.GetStatistic())
                yield return statistic;
        }
    }
}
