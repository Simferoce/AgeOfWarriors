using UnityEngine;

namespace Game.Statistics
{
    public class StatisticDefinitionRepository
    {
        public static StatisticDefinitionRepository Instance
        {
            get
            {
                if (instance == null)
                    instance = new StatisticDefinitionRepository();

                return instance;
            }
        }

        private static StatisticDefinitionRepository instance;

#if UNITY_EDITOR
        private static StatisticDefinitionEditorProvider statisticDefinitionEditorProvider = new StatisticDefinitionEditorProvider();
#endif

        public StatisticDefinition GetById(StatisticIdentifiant identifiant)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return statisticDefinitionEditorProvider.GetById(identifiant);
#endif

            return StatisticDefinitionRuntimeProvider.Instance.GetById(identifiant);
        }
    }
}