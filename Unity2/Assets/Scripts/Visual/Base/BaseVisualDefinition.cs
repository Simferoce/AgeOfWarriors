using AgeOfWarriors.Core;
using UnityEngine;

namespace AgeOfWarriors.Visual
{
    [CreateAssetMenu(menuName = "Definition/BaseVisualDefinition", fileName = "BaseVisualDefinition")]
    public class BaseVisualDefinition : VisualDefinition
    {
        [SerializeField] private GameObject visual;

        public override EntityVisual Instantiate(Entity entity)
        {
            GameObject baseGameObject = Instantiate(visual);
            BaseVisual baseVisual = baseGameObject.GetComponent<BaseVisual>();
            baseVisual.Initialize(entity as Base);

            return baseVisual;
        }

        public override bool IsVisualFor(Entity entity)
        {
            return entity is Base;
        }
    }
}
