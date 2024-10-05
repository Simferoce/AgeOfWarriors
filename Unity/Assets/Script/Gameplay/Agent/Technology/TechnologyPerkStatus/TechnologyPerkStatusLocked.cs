namespace Game
{
    public class TechnologyPerkStatusLocked : TechnologyPerkStatus
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

        public TechnologyPerkStatusLocked(LockedReason reason)
        {
            Reason = reason;
        }
    }
}
