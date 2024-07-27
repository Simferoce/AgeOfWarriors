using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

        public delegate void PerkAcquired(TechnologyPerkDefinition technologyPerkDefinition);

        [SerializeField] private float maxTechnology;
        [SerializeField] private TechnologyTreeDefinition technologyTreeDefinition;

        public event PerkAcquired OnPerkAcquired;

        public float CurrentTechnology { get; set; }
        public float CurrentLevel { get; set; }
        public float CurrentTechnologyNormalized { get => CurrentTechnology / maxTechnology; }
        public List<TechnologyPerkDefinition> PerksUnlocked { get => perksUnlocked; }
        public float MaxTechnology { get => maxTechnology; set => maxTechnology = value; }
        public TechnologyTreeDefinition TechnologyTreeDefinition { get => technologyTreeDefinition; set => technologyTreeDefinition = value; }

        private Agent agent;
        private List<TechnologyPerkDefinition> perksUnlocked = new List<TechnologyPerkDefinition>();

        public event Action OnLeveledUp;

        public void Initialize(Agent agent)
        {
            this.agent = agent;
        }

        public void Update()
        {
            foreach (AgentObject agentObject in AgentObject.All)
            {
                if (agentObject is Character character && agentObject.Agent == agent)
                {
                    CurrentTechnology += Level.Instance.TechnologyGainMultiplier * character.TechnologyGainPerSecond * Time.deltaTime;
                }
            }

            if (CurrentTechnology >= maxTechnology)
            {
                CurrentTechnology -= maxTechnology;
                LevelUp();
            }
        }

        public void Acquire(TechnologyPerkDefinition definition)
        {
            PerksUnlocked.Add(definition);
            OnPerkAcquired?.Invoke(definition);
        }

        public TechnologyPerkStatus GetStatus(TechnologyPerkDefinition definition)
        {
            if (perksUnlocked.Contains(definition))
                return new TechnologyPerkStatusUnlocked();

            int currentRow = UnlockableRow();
            if (currentRow == -1)
                return new TechnologyPerkStatusLocked(TechnologyPerkStatusLocked.LockedReason.TreeCompleted);

            int perkRow = GetRow(definition);
            Assert.IsTrue(perkRow != -1, $"The element {definition} is not in the technology tree of {technologyTreeDefinition}");

            if (perkRow < currentRow)
                return new TechnologyPerkStatusLocked(TechnologyPerkStatusLocked.LockedReason.AlreadyChoosePerkForRow);

            if (technologyTreeDefinition.Rows[perkRow].Level > CurrentLevel)
                return new TechnologyPerkStatusLocked(TechnologyPerkStatusLocked.LockedReason.LevelRequirementNotSatisfied);

            if (perkRow > currentRow)
                return new TechnologyPerkStatusLocked(TechnologyPerkStatusLocked.LockedReason.PerkRowHasNotBeenUnlocked);

            if (!definition.IsUnlockable(agent))
                return new TechnologyPerkStatusLocked(TechnologyPerkStatusLocked.LockedReason.PerkDoesNotMeetRequirement);

            return new TechnologyPerkStatusUnlockable();
        }

        public int GetRow(TechnologyPerkDefinition definition)
        {
            for (int i = 0; i < technologyTreeDefinition.Rows.Count; i++)
            {
                TechnologyTreeDefinition.Row row = technologyTreeDefinition.Rows[i];
                foreach (TechnologyPerkDefinition rowElement in row.Nodes)
                {
                    if (definition == rowElement)
                        return i;
                }
            }

            return -1;
        }

        public int UnlockableRow()
        {
            if (perksUnlocked.Count == 0)
                return 0;

            for (int i = technologyTreeDefinition.Rows.Count - 1; i >= 0; --i)
            {
                bool isUnlocked = false;
                foreach (TechnologyPerkDefinition technologyPerkDefinition in technologyTreeDefinition.Rows[i].Nodes)
                {
                    if (perksUnlocked.Contains(technologyPerkDefinition))
                    {
                        isUnlocked = true;
                        continue;
                    }
                }

                if (isUnlocked)
                    return i + 1;
            }

            return -1;
        }

        private void LevelUp()
        {
            CurrentLevel++;
            OnLeveledUp?.Invoke();
        }
    }
}
