using AgeOfWarriors.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgeOfWarriors.Visual
{
    public class VisualDefinitionRepository : MonoBehaviour
    {
        [SerializeField] private List<VisualDefinition> visualDefinitions;

        public VisualDefinition GetCorrespondingVisual(Entity entity)
        {
            return visualDefinitions.FirstOrDefault(x => x.IsVisualFor(entity));
        }
    }
}
