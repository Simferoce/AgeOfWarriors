using System.Collections.Generic;
using UnityEngine.Assertions;
using static Game.TechnologyHandler;

namespace Game
{
    public class TechnologyTree
    {
        public event System.Action<TechnologyPerkDefinition> OnPerkAcquired;

        public List<TechnologyPerkDefinition> PerksUnlocked { get => perksUnlocked; }
        public TechnologyTreeDefinition TechnologyTreeDefinition { get => technologyTreeDefinition; set => technologyTreeDefinition = value; }
        public Agent Agent { get => agent; set => agent = value; }

        private TechnologyTreeDefinition technologyTreeDefinition;
        private List<TechnologyPerkDefinition> perksUnlocked = new List<TechnologyPerkDefinition>();
        private Agent agent;

        public TechnologyTree(Agent agent, TechnologyTreeDefinition technologyTreeDefinition)
        {
            this.agent = agent;
            this.technologyTreeDefinition = technologyTreeDefinition;
        }

        public void Acquire(TechnologyPerkDefinition definition)
        {
            PerksUnlocked.Add(definition);
            OnPerkAcquired?.Invoke(definition);
        }

        public TechnologyPerkDefinition GetFirst(System.Func<TechnologyPerkDefinition, bool> selector)
        {
            foreach (TechnologyPerkDefinition unlockedPerk in PerksUnlocked)
            {
                if (selector.Invoke(unlockedPerk))
                    return unlockedPerk;
            }

            return null;
        }

        public bool Has(TechnologyPerkDefinition technologyPerkDefinition)
        {
            return PerksUnlocked.Contains(technologyPerkDefinition);
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

            if (technologyTreeDefinition.Rows[perkRow].Level > agent.Technology.CurrentLevel)
                return new TechnologyPerkStatusLocked(TechnologyPerkStatusLocked.LockedReason.LevelRequirementNotSatisfied);

            if (perkRow > currentRow)
                return new TechnologyPerkStatusLocked(TechnologyPerkStatusLocked.LockedReason.PerkRowHasNotBeenUnlocked);

            if (!definition.IsUnlockable(agent))
                return new TechnologyPerkStatusLocked(TechnologyPerkStatusLocked.LockedReason.PerkDoesNotMeetRequirement);

            return new TechnologyPerkStatusUnlockable();
        }

        private int GetRow(TechnologyPerkDefinition definition)
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

        private int UnlockableRow()
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
    }
}
