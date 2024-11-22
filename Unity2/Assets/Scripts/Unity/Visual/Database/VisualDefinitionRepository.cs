using AgeOfWarriors.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgeOfWarriors.Visual
{
    public class VisualDefinitionRepository : IDisposable
    {
        private AddressablesExtensions.AddressablesHandle<VisualDefinition> handle;
        private List<VisualDefinition> visualDefinitions;

        public async Awaitable Initialize()
        {
            handle = await AddressablesExtensions.LoadAssetsAsync<VisualDefinition>("Visual");
            visualDefinitions = handle.Result.ToList();
        }

        public void Dispose()
        {
            handle.Dispose();
        }

        public VisualDefinition GetCorrespondingVisual(Entity entity)
        {
            return visualDefinitions.FirstOrDefault(x => x.IsVisualFor(entity));
        }
    }
}
