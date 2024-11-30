using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public abstract class Modifier : IDisposable
    {
        public delegate void OnDisposeDelegate(Modifier modifier);
        public event OnDisposeDelegate OnDispose;

        public List<ModifierElement> modifierElements = new List<ModifierElement>();
        public abstract ModifierDefinition Definition { get; }

        public virtual bool? IsInvulnerable => null;
        public virtual bool? IsStagger => null;
        public virtual bool? IsConfused => null;

        public ModifierHandler Modifiable { get => modifiable; set => modifiable = value; }
        public ModifierApplier Source { get; private set; }
        public virtual bool Show => Definition.Show;
        public StatisticRegistry StatisticRegistry { get; set; } = new StatisticRegistry();

        protected ModifierHandler modifiable;
        protected List<ModifierParameter> parameters;

        public virtual void Initialize(ModifierHandler modifiable, ModifierApplier source, List<ModifierParameter> parameters)
        {
            this.parameters = parameters;
            this.Source = source;
            this.modifiable = modifiable;

            foreach (ModifierElement element in modifierElements)
                element.Initialize();
        }

        public T GetParameterValue<T>(string name)
        {
            ModifierParameter modifierParameter = parameters.FirstOrDefault(x => x.Name == name);
            if (modifierParameter == null)
            {
                Debug.LogError($"Expecting a parameter with name \"{name}\". Available parameters: {string.Join(", ", modifierParameter.ToString())}");
                return default;
            }

            return modifierParameter.GetValue<T>();
        }

        public virtual string ParseDescription() { return Definition.ParseDescription(); }

        public virtual void Update()
        {
            bool end = false;
            foreach (ModifierElement element in modifierElements)
            {
                end |= element.Update();
            }

            if (end)
                modifiable.RemoveModifier(this);
        }

        public virtual float? GetPercentageRemainingDuration()
        {
            CharacterModifierTimeElement modifierElement = (CharacterModifierTimeElement)modifierElements.FirstOrDefault(x => x is CharacterModifierTimeElement);

            if (modifierElement == null)
                return null;

            return Mathf.Clamp01(modifierElement.RemaingDuration / modifierElement.Duration);
        }

        public virtual float? GetStack()
        {
            StackModifierElement stackModifierElement = (StackModifierElement)modifierElements.FirstOrDefault(x => x is StackModifierElement);
            return stackModifierElement?.CurrentStack;
        }

        public virtual void Refresh()
        {
            foreach (ModifierElement element in modifierElements)
            {
                element.Refresh();
            }
        }

        public virtual void Dispose()
        {
            OnDispose?.Invoke(this);
        }

        public Modifier With(List<ModifierElement> modifierElements)
        {
            this.modifierElements.AddRange(modifierElements);
            return this;
        }

        public Modifier With(ModifierElement modifierElement)
        {
            this.modifierElements.Add(modifierElement);
            return this;
        }
    }

    public class Modifier<T, U> : Modifier
        where T : Modifier<T, U>
        where U : ModifierDefinition
    {
        protected U definition;

        public override ModifierDefinition Definition => definition;

        protected Modifier(U modifierDefinition)
        {
            definition = modifierDefinition;
        }

        public new T With(List<ModifierElement> modifierElements)
        {
            this.modifierElements.AddRange(modifierElements);
            return (T)this;
        }

        public new T With(ModifierElement modifierElement)
        {
            this.modifierElements.Add(modifierElement);
            return (T)this;
        }
    }
}
