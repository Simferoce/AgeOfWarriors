namespace Game.Technology
{
    public class LockedTechnologyPerkStatus : TechnologyPerkStatus
    {
        public enum LockedReason
        {
            TreeCompleted,
            AlreadyChoosePerkForRow,
            PerkRowHasNotBeenUnlocked,
            PerkDoesNotMeetRequirement,
            LevelRequirementNotSatisfied
        }

        public LockedReason Reason { get; set; }

        public LockedTechnologyPerkStatus(LockedReason reason)
        {
            Reason = reason;
        }
    }
}
