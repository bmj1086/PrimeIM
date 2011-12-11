namespace PrimeIM.Data
{
    public class BuddyListEventArgs
    {
        public Buddy Buddy;
        public EventType EventType;

        public BuddyListEventArgs(Buddy buddyChanged, EventType eventType)
        {
            Buddy = buddyChanged;
            EventType = eventType;
        }

        
    }

    public enum EventType
    {
        Add,
        Update,
        DoubleClicked,
        Remove,
    }
}