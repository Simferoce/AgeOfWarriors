using AgeOfWarriors;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AgeOfWarriors.Visual
{
    public class VisualApplication : IDisposable
    {
        private VisualDefinitionRepository visualDefinitionRepository;
        private List<EntityVisual> visuals = new List<EntityVisual>();
        private Game game;

        public async Awaitable Initialize(Game game)
        {
            this.game = game;
            visualDefinitionRepository = new VisualDefinitionRepository();
            await visualDefinitionRepository.Initialize();
        }

        public void Dispose()
        {
            visualDefinitionRepository.Dispose();
        }

        public void Update()
        {
            Refresh();
            foreach (EntityVisual visual in visuals)
            {
                visual.Refresh();
            }
        }

        private void Refresh()
        {
            foreach (Entity entity in game.Entities)
            {
                if (!visuals.Any(x => x.Entity == entity))
                {
                    VisualDefinition visualDefinition = visualDefinitionRepository.GetCorrespondingVisual(entity);
                    if (visualDefinition == null)
                        continue;

                    EntityVisual entityVisual = visualDefinition.Instantiate(entity);
                    entityVisual.Refresh();
                    visuals.Add(entityVisual);
                }
            }

            for (int i = visuals.Count - 1; i >= 0; i--)
            {
                EntityVisual visual = visuals[i];
                if (!game.Entities.Any(x => x == visual.Entity))
                {
                    GameObject.Destroy(visual);
                    visuals.Remove(visual);
                }
            }
        }
    }
}
