using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Blocker : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Init()
        {
            All = new List<Blocker>();
        }

        public static List<Blocker> All = new List<Blocker>();

        [SerializeReference, SubclassSelector] private TargetCriteria blocking;
        [SerializeField] private Collider2D collider;
        [SerializeField] private AgentObject owner;

        public Collider2D Collider => collider;
        public bool IsActive => owner.IsActive;

        private void Awake()
        {
            All.Add(this);
        }

        private void OnDestroy()
        {
            All.Remove(this);
        }

        public bool IsBlocking(AgentObject agentObject)
        {
            if (blocking == null)
                return false;

            return blocking.Execute(owner, agentObject, this);
        }
    }
}
