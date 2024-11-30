using Game.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [RequireComponent(typeof(AttackFactory))]
    public class ModifierEntity : Entity<ModifierDefinition>
    {
        [SerializeField] private bool visibleByDefault = true;
        [SerializeReference, SubclassSelector] private List<ModifierBehaviour> behaviours;

        public delegate void OnRemovedDelegate(ModifierEntity modifier);
        public event OnRemovedDelegate OnRemoved;

        public ModifierHandler Target { get; set; }
        public ModifierApplier Applier { get; set; }
        public bool IsVisible { get => visibleByDefault; }
        public List<ModifierBehaviour> Behaviours { get => behaviours; set => behaviours = value; }
        public List<ModifierParameter> Parameters { get => parameters; set => parameters = value; }

        private List<ModifierParameter> parameters;

        public void Initialize(ModifierHandler target, ModifierApplier applier, params ModifierParameter[] parameters)
        {
            this.parameters = parameters.ToList();
            this.Target = target;
            this.Applier = applier;
            this.Parent = target.Entity;
            this.transform.parent = target.transform;

            base.Initialize();

            foreach (ModifierParameter<ModifierBehaviour> modifierBehaviorParameter in parameters.OfType<ModifierParameter<ModifierBehaviour>>())
                Behaviours.Add(modifierBehaviorParameter.GetValue());

            foreach (ModifierBehaviour modifierBehaviour in Behaviours)
                modifierBehaviour.Initialize(this);
        }

        private void Update()
        {
            bool kill = false;
            foreach (ModifierBehaviour modifierBehaviour in Behaviours)
            {
                if (modifierBehaviour.Update() == ModifierBehaviour.Result.Dead)
                    kill = true;
            }

            if (kill)
                Kill();
        }

        public void Refresh(params ModifierParameter[] parameters)
        {
            foreach (ModifierBehaviour modifierBehaviour in Behaviours)
                modifierBehaviour.Refresh();
        }

        public void Kill()
        {
            GameObject.Destroy(gameObject);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Target.Remove(this);
            OnRemoved?.Invoke(this);

            foreach (ModifierBehaviour modifierBehaviour in Behaviours)
                modifierBehaviour.Dispose();
        }

        public string ParseDescription()
        {
            return definition.ParseDescription(this);
        }
    }
}
