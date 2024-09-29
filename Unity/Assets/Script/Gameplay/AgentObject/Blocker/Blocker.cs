using UnityEngine;

namespace Game
{
    public class Blocker : MonoBehaviour, IComponent
    {
        [SerializeField] private bool blockAllies = true;
        [SerializeField] private bool blockEnemies = true;
        [SerializeField] private Collider2D hitbox;

        public Entity Entity { get; set; }
        public bool IsActive => Entity is not AgentObject agentObject || agentObject.IsActive;
        public Collider2D Hitbox { get => hitbox; set => hitbox = value; }

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
            if (entity is not Character character)
                return false;

            if (Entity is not AgentObject agentObject)
                return false;

            return character.Agent.Faction == agentObject.Agent.Faction
                && character.Priority > agentObject.Priority;
        }

        public bool BlockingEnemies(Entity entity)
        {
            if (entity is not Character character)
                return false;

            if (Entity is not AgentObject agentObject)
                return false;

            return character.Agent.Faction != agentObject.Agent.Faction;
        }
    }
}