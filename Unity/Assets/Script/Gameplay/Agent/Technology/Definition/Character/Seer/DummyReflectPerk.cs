using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DummyReflectPerk", menuName = "Definition/Technology/Seer/DummyReflectPerk")]
    public class DummyReflectPerk : ModifierDefinition
    {
        public class Modifier : Modifier<Modifier, DummyReflectPerk>
        {
            //private Ownership ownership;

            public Modifier(DummyReflectPerk modifierDefinition) : base(modifierDefinition)
            {
                //ownership = modifiable.AddOrGetCachedComponent<Ownership>();
                //ownership.OnChildAdded += Ownership_OnChildAdded;
            }

            //private void Ownership_OnChildAdded(Ownership ownership)
            //{
            //    //Assuming that all sub character are dummy, might have to change in the futur.
            //    if (ownership.TryGetCachedComponent<Character>(out Character character))
            //    {
            //        ModifierHandler subordinateModifiable = character.GetCachedComponent<ModifierHandler>();
            //        subordinateModifiable.AddModifier(new ReflectDamageModifierDefinition.Modifier(
            //            character.GetCachedComponent<ModifierHandler>(),
            //            definition.reflectDamageModifierDefinition,
            //            character,
            //            definition.damage));
            //    }
            //}

            //public override void Dispose()
            //{
            //    base.Dispose();

            //    ownership.OnChildAdded -= Ownership_OnChildAdded;
            //}
        }

        [SerializeField] private ReflectDamageModifierDefinition reflectDamageModifierDefinition;
        [SerializeField] private float damage;

        public override Game.Modifier Instantiate()
        {
            return new Modifier(this);
        }

        public override string ParseDescription()
        {
            return string.Format(Description, damage);
        }
    }
}
