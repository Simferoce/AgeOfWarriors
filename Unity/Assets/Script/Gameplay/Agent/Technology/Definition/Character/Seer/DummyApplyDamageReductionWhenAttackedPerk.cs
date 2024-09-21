using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "DummyApplyDamageReductionWhenAttackedPerk", menuName = "Definition/Technology/Seer/DummyApplyDamageReductionWhenAttackedPerk")]
    public class DummyApplyDamageReductionWhenAttackedPerk : CharacterTechnologyPerkDefinition
    {
        public class Modifier : Modifier<Modifier, DummyApplyDamageReductionWhenAttackedPerk>
        {
            //private Ownership ownership;

            public Modifier(ModifierHandler modifiable, DummyApplyDamageReductionWhenAttackedPerk modifierDefinition, IModifierSource source) : base(modifiable, modifierDefinition, source)
            {
                //ownership = modifiable.AddOrGetCachedComponent<Ownership>();
                //ownership.OnChildAdded += Ownership_OnChildAdded;
                throw new System.Exception("Ownership not implemented");
            }

            //private void Ownership_OnChildAdded(Ownership ownership)
            //{
            //    //Assuming that all sub character are dummy, might have to change in the futur.
            //    if (ownership.TryGetCachedComponent<Character>(out Character character))
            //    {
            //        ModifierHandler subordinateModifiable = character.Entity.GetCachedComponent<ModifierHandler>();

            //        DamageDealtReductionModifierDefinition.Modifier.Instancier instancier = new DamageDealtReductionModifierDefinition.Modifier.Instancier();
            //        instancier.Amount = definition.amount;
            //        instancier.Definition = definition.damageDealtReductionDefinition;
            //        instancier.Duration = definition.duration;

            //        subordinateModifiable.AddModifier(new ApplyEffectWhenHitModifierDefinition.Modifier(
            //            character.Entity.GetCachedComponent<ModifierHandler>(),
            //            definition.applyEffectWhenHitModifierDefinition,
            //            character,
            //            instancier));
            //    }
            //}

            //public override void Dispose()
            //{
            //    base.Dispose();

            //    ownership.OnChildAdded -= Ownership_OnChildAdded;
            //}
        }

        [SerializeField] private ApplyEffectWhenHitModifierDefinition applyEffectWhenHitModifierDefinition;
        [SerializeField] private DamageDealtReductionModifierDefinition damageDealtReductionDefinition;
        [SerializeField, Range(0, 1)] private float amount;
        [SerializeField] private float duration;

        public override Game.Modifier GetModifier(ModifierHandler modifiable)
        {
            return new Modifier(modifiable, this, modifiable.Entity.GetCachedComponent<IModifierSource>());
        }
    }
}
