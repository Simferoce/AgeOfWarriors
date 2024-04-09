using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GenericAbilityDefinition", menuName = "Definition/Ability/GenericAbilityDefinition")]
    public class GenericAbilityDefinition : AbilityDefinition
    {
        [SerializeReference, SubclassSelector] private CharacterAbility characterAbility;

        public override CharacterAbility GetAbility()
        {
            CharacterAbility clone = characterAbility.Clone();
            clone.Definition = this;
            return clone;
        }
    }
}
