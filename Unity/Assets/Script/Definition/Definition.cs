using UnityEngine;

namespace Game
{
    public class Definition : ScriptableObject
    {
        [SerializeField]
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return this.name; }
        }
    }
}
