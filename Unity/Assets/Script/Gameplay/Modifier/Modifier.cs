using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public abstract class Modifier : IModifier, IDisposable
    {
        public List<ModifierElement> modifierElements = new List<ModifierElement>();
        public abstract ModifierDefinition Definition { get; }

        public virtual float? SpeedPercentage => null;
        public virtual float? Defense => null;
        public virtual float? MaxHealth => null;
        public virtual float? AttackSpeedPercentage => null;
        public virtual float? AttackPower => null;
        public virtual bool? IsInvulnerable => null;
        public virtual bool? IsStagger => null;
        public virtual bool? IsConfused => null;
        public virtual float? RangedDamageReduction => null;
        public virtual float? DamageDealtReduction => null;
        public virtual float? DamageDealtAgainstWeak => null;
        public virtual float? IncreaseDamageTaken => null;
        public virtual float? DefenseReduction => null;

        public IModifiable Modifiable { get => modifiable; set => modifiable = value; }
        public IModifierSource Source { get; }
        public virtual bool Show => Definition.Show;

        protected IModifiable modifiable;

        protected Modifier(IModifiable modifiable, IModifierSource source = null)
        {
            this.Source = source;
            this.modifiable = modifiable;

            if (Source != null)
                Source.AddModifier(this);
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
                Source.RemoveModifier(this);
        }
    }

    public abstract class Modifier<T, U> : Modifier
        where T : Modifier<T, U>
        where U : ModifierDefinition
    {
        protected U definition;

        public override ModifierDefinition Definition => definition;

        protected Modifier(IModifiable modifiable, U modifierDefinition, IModifierSource source) : base(modifiable, source)
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
