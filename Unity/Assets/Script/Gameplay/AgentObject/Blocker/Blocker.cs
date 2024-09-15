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

        public bool IsBlocking(Entity entity)
        {
            return IsInContact(entity)
                && (!blockAllies || BlockingAllies(entity))
                && (!blockEnemies || BlockingEnemies(entity));
        }

        public bool IsInContact(Entity entity)
        {
            if (entity is not Character character)
                return false;

            return hitbox.IsTouching(character.Hitbox);
        }

        public bool BlockingAllies(Entity entity)
        {
            if (entity is not Character character)
                return false;

            if (Entity is not AgentObject agentObject)
                return false;

            return character.OriginalFaction == agentObject.OriginalFaction
                && character.Priority > agentObject.Priority;
        }

        public bool BlockingEnemies(Entity entity)
        {
            if (entity is not Character character)
                return false;

            if (Entity is not AgentObject agentObject)
                return false;

            return character.OriginalFaction != agentObject.OriginalFaction;
        }
    }
}