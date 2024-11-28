using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public abstract class Modifier : IDisposable
    {
        public List<ModifierElement> modifierElements = new List<ModifierElement>();
        public abstract ModifierDefinition Definition { get; }

        public virtual bool? IsInvulnerable => null;
        public virtual bool? IsStagger => null;
        public virtual bool? IsConfused => null;

        public ModifierHandler Modifiable { get => modifiable; set => modifiable = value; }
        public IModifierSource Source { get; }
        public virtual bool Show => Definition.Show;
        public StatisticRegistry StatisticRegistry { get; set; } = new StatisticRegistry();

        protected ModifierHandler modifiable;

        protected Modifier(ModifierHandler modifiable, IModifierSource source = null)
        {
            this.Source = source;
            this.modifiable = modifiable;

            if (Source != null)
                Source.AddAppliedModifier(this);
        }

        public void Initialize()
        {
            foreach (ModifierElement element in modifierElements)
                element.Initialize();
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
            if (Source != null)
                Source.RemoveAppliedModifier(this);
        }
    }

    public abstract class Modifier<T, U> : Modifier
        where T : Modifier<T, U>
        where U : ModifierDefinition
    {
        protected U definition;

        public override ModifierDefinition Definition => definition;

        protected Modifier(ModifierHandler modifiable, U modifierDefinition, IModifierSource source) : base(modifiable, source)
        {
            definition = modifierDefinition;
        }

        public T With(List<ModifierElement> modifierElements)
        {
            this.modifierElements.AddRange(modifierElements);
            return (T)this;
        }

        public T With(ModifierElement modifierElement)
        {
            this.modifierElements.Add(modifierElement);
            return (T)this;
        }
    }
}
