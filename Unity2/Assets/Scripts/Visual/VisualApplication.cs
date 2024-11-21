using AgeOfWarriors.Core;
using AgeOfWarriors.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace AgeOfWarriors.Visual
{
    public class VisualApplication : MonoBehaviour
    {
        [SerializeField] private VisualDefinitionRepository characterVisualDefinitionRepository;

        private List<EntityVisual> visuals = new List<EntityVisual>();

        public void Initialize(GameApplication application)
        {
            application.Game.EventChannel.Subscribe<EntityCreatedEvent>(EntityCreated);
        }

        private void EntityCreated(EntityCreatedEvent evt)
        {
            VisualDefinition visualDefinition = characterVisualDefinitionRepository.GetCorrespondingVisual(evt.Entity);
            if (visualDefinition != null)
            {
                EntityVisual entityVisual = visualDefinition.Instantiate(evt.Entity);
                entityVisual.Refresh();
                visuals.Add(entityVisual);
            }
        }

        private void Update()
        {
            foreach (EntityVisual visual in visuals)
            {
                visual.Refresh();
            }
        }
    }
}
