using Game.Agent;
using Game.Character;
using UnityEngine;

namespace Game.Components
{
    public class Blocker : MonoBehaviour
    {
        [SerializeField] private bool blockAllies = true;
        [SerializeField] private bool blockEnemies = true;
        [SerializeField] private Collider2D hitbox;

        public Entity Entity { get; set; }
        public Collider2D Hitbox { get => hitbox; set => hitbox = value; }

        private void Awake()
        {
            Entity = GetComponentInParent<Entity>();
        }

        public bool IsBlocking(Entity entity)
        {
            if (!IsInContact(entity))
                return false;

            if (blockAllies && BlockingAllies(entity))
                return true;

            if (blockEnemies && BlockingEnemies(entity))
                return true;

            return false;
        }

        public bool IsInContact(Entity entity)
        {
            if (!entity.TryGetCachedComponent<Blocker>(out Blocker blocker))
                return false;

            return Hitbox.IsTouching(blocker.Hitbox);
        }

        public bool BlockingAllies(Entity entity)
        {
            if (entity is not CharacterEntity character)
                return false;

            if (!Entity.TryGetCachedComponent<AgentIdentity>(out AgentIdentity selfIdentity))
                return false;

            if (!character.TryGetCachedComponent<AgentIdentity>(out AgentIdentity otherIdentity))
                return false;

            return otherIdentity.Agent.Faction == selfIdentity.Agent.Faction
                && otherIdentity.Priority > selfIdentity.Priority;
        }

        public bool BlockingEnemies(Entity entity)
        {
            if (entity is not CharacterEntity character)
                return false;

            if (!Entity.TryGetCachedComponent<AgentIdentity>(out AgentIdentity selfIdentity))
                return false;

            if (!character.TryGetCachedComponent<AgentIdentity>(out AgentIdentity otherIdentity))
                return false;

            return selfIdentity.Agent.Faction != otherIdentity.Agent.Faction;
        }
    }
}