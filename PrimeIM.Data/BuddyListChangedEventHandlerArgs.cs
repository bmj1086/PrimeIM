using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PrimeIM.Data
{
    public class BuddyListChangedEventHandlerArgs
    {
        public BuddyListChangedEventHandlerArgs(BuddyListChangedAction action, IList<Buddy> changedBuddies )
        {
            if (changedBuddies == null || changedBuddies.Count() == 0)
                throw new NoNullAllowedException("changedBuddies cannot be null or empty.");
            
            Action = action;
            ChangedBuddies = changedBuddies;
        }

        public BuddyListChangedAction Action { get; private set; }
        public IList<Buddy> ChangedBuddies { get; private set; }
    }
}