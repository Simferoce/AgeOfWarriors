using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Game.Technology
{
    [Serializable]
    public class RowTechnologyRequirement : TechnologyRequirement
    {
        public override bool Execute(TechnologyPerkDefinition technologyPerkDefinition, TechnologyTree technologyTree)
        {
            int currentRow = technologyTree.GetRow(technologyPerkDefinition);
            Assert.IsTrue(currentRow >= 0, $"Expecting could not get the row of {technologyPerkDefinition} in {technologyTree.TechnologyTreeDefinition}");

            List<TechnologyPerkDefinition> rowTechnologyPerkDefinitions = technologyTree.GetTechnologyPerkOfRow(currentRow);
            foreach (TechnologyPerkDefinition rowTechnologyPerkDefinition in rowTechnologyPerkDefinitions)
            {
                if (technologyTree.IsUnlocked(rowTechnologyPerkDefinition))
                    return false;
            }

            return true;
        }

        public override string Format(TechnologyPerkDefinition technologyPerkDefinition, TechnologyTree technologyTree)
        {
            return string.Empty;
        }
    }
}
