﻿using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DummyReflectPerk", menuName = "Definition/Technology/Seer/DummyReflectPerk")]
    public class DummyReflectPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, DummyReflectPerk>
        {
            private Ownership ownership;

            public Modifier(IModifiable modifiable, DummyReflectPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                ownership = modifiable.AddOrGetCachedComponent<Ownership>();
                ownership.OnChildAdded += Ownership_OnChildAdded;
            }

            private void Ownership_OnChildAdded(Ownership ownership)
            {
                //Assuming that all sub character are dummy, might have to change in the futur.
                if (ownership.TryGetCachedComponent<Character>(out Character character))
                {
                    IModifiable subordinateModifiable = character.GetCachedComponent<IModifiable>();
                    subordinateModifiable.AddModifier(new ReflectDamageModifierDefinition.Modifier(
                        character.GetCachedComponent<IModifiable>(),
                        definition.reflectDamageModifierDefinition,
                        character,
                        definition.damage));
                }
            }

            public override void Dispose()
            {
                base.Dispose();

                ownership.OnChildAdded -= Ownership_OnChildAdded;
            }
        }

        [SerializeField] private ReflectDamageModifierDefinition reflectDamageModifierDefinition;
        [SerializeField] private float damage;

        public override Game.Modifier GetModifier(IModifiable modifiable)
        {
            return new Modifier(modifiable, this, modifiable.GetCachedComponent<IModifierSource>());
        }

        public override string ParseDescription()
        {
            return string.Format(Description, damage);
        }
    }
}