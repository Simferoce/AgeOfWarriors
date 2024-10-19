using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Game.Technology
{
    public class TechnologyTree
    {
        public event System.Action<TechnologyPerkDefinition> OnPerkAcquired;

        public List<TechnologyPerkDefinition> PerksUnlocked { get => perksUnlocked; }
        public TechnologyTreeDefinition TechnologyTreeDefinition { get => technologyTreeDefinition; set => technologyTreeDefinition = value; }
        public Agent.AgentEntity Agent { get => agent; set => agent = value; }

        private TechnologyTreeDefinition technologyTreeDefinition;
        private List<TechnologyPerkDefinition> perksUnlocked = new List<TechnologyPerkDefinition>();
        private Agent.AgentEntity agent;

        public TechnologyTree(Agent.AgentEntity agent, TechnologyTreeDefinition technologyTreeDefinition)
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
            if (IsUnlocked(definition))
                return new TechnologyPerkStatusUnlocked();

            int currentRow = UnlockableRow();
            if (currentRow == -1)
                return new LockedTechnologyPerkStatus(LockedTechnologyPerkStatus.LockedReason.TreeCompleted);

            int perkRow = GetRow(definition);
            Assert.IsTrue(perkRow != -1, $"The element {definition} is not in the technology tree of {technologyTreeDefinition}");

            if (perkRow < currentRow)
                return new LockedTechnologyPerkStatus(LockedTechnologyPerkStatus.LockedReason.AlreadyChoosePerkForRow);

            if (technologyTreeDefinition.Rows[perkRow].Level > agent.Technology.CurrentLevel)
                return new LockedTechnologyPerkStatus(LockedTechnologyPerkStatus.LockedReason.LevelRequirementNotSatisfied);

            if (perkRow > currentRow)
                return new LockedTechnologyPerkStatus(LockedTechnologyPerkStatus.LockedReason.PerkRowHasNotBeenUnlocked);

            if (!definition.IsUnlockable(this))
                return new LockedTechnologyPerkStatus(LockedTechnologyPerkStatus.LockedReason.PerkDoesNotMeetRequirement);

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

        public bool IsUnlocked(TechnologyPerkDefinition definition)
        {
            return perksUnlocked.Contains(definition);
        }

        public List<TechnologyPerkDefinition> GetTechnologyPerkOfRow(int row)
        {
            Assert.IsTrue(row >= 0, "The row index must be higher than 0.");
            Assert.IsTrue(row < technologyTreeDefinition.Rows.Count, $"The row index is higher than the number of row in {technologyTreeDefinition}");
            return technologyTreeDefinition.Rows[row].Nodes;
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
