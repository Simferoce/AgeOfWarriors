namespace AgeOfWarriors
{
    public struct EntityCreatedEvent
    {
        public Entity Entity { get; set; }

        public EntityCreatedEvent(Entity entity)
        {
            Entity = entity;
        }
    }
}
