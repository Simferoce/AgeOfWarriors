using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class TechnologyHandler
    {
        public abstract class TechnologyPerkStatus
        {

        }

        public class TechnologyPerkStatusUnlockable : TechnologyPerkStatus
        {

        }

        public class TechnologyPerkStatusUnlocked : TechnologyPerkStatus
        {

        }

        public class TechnologyPerkStatusLocked : TechnologyPerkStatus
        {
            public enum LockedReason
            {
                TreeCompleted,
                AlreadyChoosePerkForRow,
                PerkRowHasNotBeenUnlocked,
                PerkDoesNotMeetRequirement,
                LevelRequirementNotSatisfied
            }

            public LockedReason Reason { get; set; }

            public TechnologyPerkStatusLocked(LockedReason reason)
            {
                Reason = reason;
            }
        }

        public class TechnologyPerkStatusNotInTree : TechnologyPerkStatus
        {

        }

        public delegate void PerkAcquired(TechnologyPerkDefinition technologyPerkDefinition);

        [SerializeField] private float maxTechnology;
        [SerializeField] private List<TechnologyTreeDefinition> technologyTreeDefinitions;

        public event PerkAcquired OnPerkAcquired;

        public float CurrentTechnology { get; set; }
        public float CurrentLevel { get; set; }
        public float CurrentTechnologyNormalized { get => CurrentTechnology / maxTechnology; }
        public float MaxTechnology { get => maxTechnology; set => maxTechnology = value; }
        public List<TechnologyTree> TechnologyTrees { get => technologyTrees; set => technologyTrees = value; }

        private Agent agent;
        private List<TechnologyTree> technologyTrees = new List<TechnologyTree>();

        public event Action OnLeveledUp;

        public void Initialize(Agent agent)
        {
            this.agent = agent;

            foreach (TechnologyTreeDefinition technologyTreeDefinition in technologyTreeDefinitions)
            {
                technologyTrees.Add(technologyTreeDefinition.Instantiate(agent));
            }
        }

        public void Update()
        {
            foreach (AgentObject agentObject in AgentObject.All)
            {
                if (agentObject is Character character && agentObject.Agent == agent)
                {
                    CurrentTechnology += Level.Instance.TechnologyGainMultiplier * character.TechnologyPerSecond * Time.deltaTime;
                }
            }

            if (CurrentTechnology >= maxTechnology)
            {
                CurrentTechnology -= maxTechnology;
                LevelUp();
            }
        }

        private void LevelUp()
        {
            CurrentLevel++;
            OnLeveledUp?.Invoke();
        }

        public bool Has(TechnologyPerkDefinition technologyPerkDefinition)
        {
            foreach (TechnologyTree technologyTree in technologyTrees)
            {
                if (technologyTree.Has(technologyPerkDefinition))
                    return true;
            }

            return false;
        }

        public TechnologyPerkStatus GetStatus(TechnologyPerkDefinition technologyPerkDefinition)
        {
            foreach (TechnologyTree technologyTree in technologyTrees)
            {
                if (technologyTree.Has(technologyPerkDefinition))
                    return technologyTree.GetStatus(technologyPerkDefinition);
            }

            return new TechnologyPerkStatusNotInTree();
        }

        public TechnologyPerkDefinition GetFirst(System.Func<TechnologyPerkDefinition, bool> selector)
        {
            foreach (TechnologyTree technologyTree in technologyTrees)
            {
                TechnologyPerkDefinition technologyPerkDefinition = technologyTree.GetFirst(selector);
                if (technologyPerkDefinition != null)
                    return technologyPerkDefinition;
            }

            return null;
        }

        public IEnumerable UnlockedPerks()
        {
            foreach (TechnologyTree technologyTree in technologyTrees)
            {
                foreach (TechnologyPerkDefinition technologyPerkDefinition in technologyTree.PerksUnlocked)
                {
                    yield return technologyPerkDefinition;
                }
            }
        }
    }
}
