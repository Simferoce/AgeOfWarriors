using Game.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Modifier
{
    [RequireComponent(typeof(AttackFactory))]
    public class ModifierEntity : Entity
    {
        [SerializeField] private bool visibleByDefault = true;
        [SerializeReference, SubclassSelector] private List<ModifierBehaviour> behaviours;

        public delegate void OnRemovedDelegate(ModifierEntity modifier);
        public event OnRemovedDelegate OnRemoved;

        public ModifierDefinition Definition { get; set; }
        public ModifierHandler Handler { get; set; }
        public ModifierApplier Applier { get; set; }
        public bool IsVisible { get => visibleByDefault; }
        public List<ModifierBehaviour> Behaviours { get => behaviours; set => behaviours = value; }
        public List<ModifierParameter> Parameters { get => parameters; set => parameters = value; }

        private List<ModifierParameter> parameters;

        public void Initialize(ModifierHandler handler, ModifierApplier applier, params ModifierParameter[] parameters)
        {
            this.parameters = parameters.ToList();
            this.Handler = handler;
            this.Applier = applier;
            this.Parent = handler.Entity;
            this.transform.parent = handler.transform;

            base.Initialize();

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
            Handler.Remove(this);
            OnRemoved?.Invoke(this);

            foreach (ModifierBehaviour modifierBehaviour in Behaviours)
                modifierBehaviour.Dispose();
        }

        public string ParseDescription()
        {
            return Definition.ParseDescription(this, null);
        }
    }
}
