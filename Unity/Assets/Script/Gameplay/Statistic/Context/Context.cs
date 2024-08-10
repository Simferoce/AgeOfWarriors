using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public class Context : IContext
    {
        public Dictionary<string, object> Elements { get; } = new Dictionary<string, object>();

        public object this[string key]
        {
            get { return Elements[key]; }
            set { Elements[key] = value; }
        }

        public void Add(string key, object value)
        {
            Elements.Add(key, value);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Elements.GetEnumerator();
        }
    }
}
