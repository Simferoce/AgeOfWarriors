using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class Context
    {
        private List<ContextElement> elements = new List<ContextElement>();

        public Context(params ContextElement[] contextElement)
        {
            elements.AddRange(contextElement);
        }

        public T GetContextElement<T>()
            where T : ContextElement
        {
            return (T)elements.FirstOrDefault(x => x is T);
        }

        public bool TryGetContextElement<T>(out T contextElement)
            where T : ContextElement
        {
            contextElement = GetContextElement<T>();
            return contextElement != null;
        }
    }
}
