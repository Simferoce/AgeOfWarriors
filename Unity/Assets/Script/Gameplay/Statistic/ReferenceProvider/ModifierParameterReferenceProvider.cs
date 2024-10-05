using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    [Serializable]
    public class ModifierParameterReferenceProvider : ReferenceProvider
    {
        public class ParameterWrapper : IStatisticContext
        {
            private List<ModifierParameter> parameters;

            public ParameterWrapper(List<ModifierParameter> parameters)
            {
                this.parameters = parameters;
            }

            public IEnumerable<Statistic> GetStatistic()
            {
                foreach (Statistic statistic in parameters.OfType<IStatisticContext>().SelectMany(x => x.GetStatistic()))
                    yield return statistic;
            }
        }

        public override object Resolve(object context)
        {
            if (!(context is Modifier modifier))
                throw new ArgumentException();

            return new ParameterWrapper(modifier.Parameters);
        }
    }
}
