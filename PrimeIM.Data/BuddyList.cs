using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.client;
using PrimeIM.Data.Comparers;
using Utility.Collections;
using agsXMPP.protocol.iq.roster;

namespace PrimeIM.Data
{
    public sealed class BuddyList : SortedUpdatableCollection<Buddy>
    {
        public static readonly BuddyList Instance = new BuddyList();
        private static readonly HashSet<RosterItem> RosterReserve = new HashSet<RosterItem>();
        
        private BuddyList()
        {
        }

        public void AddRange(IEnumerable<Buddy> items)
        {
            foreach (var buddy in items.Distinct(BuddyComparer.Instance))
                Add(buddy);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private new void Add(Buddy item)
        {
            base.Add(item);
        }

        private void Update(Presence presence)
        {
            var buddy = Get(presence.From);
            
            if (buddy == null)
                return;

            buddy.UpdateInfo(presence);

            
        }

        public bool Contains(Presence presence)
        {
            return Get(presence.From) != null;
        }

        public bool Contains(RosterItem item)
        {
            return Get(item.Jid) != null;
        }

        public Buddy Get(Presence presence)
        {
            return Get(presence.From);
        }

        public Buddy Get(Jid jid)
        {
            var rtn = this.Where(b => b.Jid.Bare.ToLower().
                Equals((jid.Bare.ToLower()))).
                SingleOrDefault();
            return rtn;
        }

        public void Remove(Presence presence)
        {
            var buddy = Get(presence);
            
            if (buddy == null)
                return;
            
            buddy.RemovePresence(presence);

            if (!buddy.IsOnline)
                Remove(buddy);
            else
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, buddy, buddy));
        }

        public void UpdateRange(IEnumerable<Presence> updateBuddies)
        {
            foreach (var presence in updateBuddies)
                Update(presence);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
