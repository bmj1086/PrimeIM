﻿using System.Collections.Generic;
using System.Linq;
using agsXMPP;
using agsXMPP.protocol.client;
using PrimeIM.Data.Comparers;
using agsXMPP.protocol.iq.roster;

namespace PrimeIM.Data
{
    public sealed class BuddyList : SortedSet<Buddy>
    {
        public delegate void BuddyListChangedEventHandler(object sender, BuddyListChangedEventHandlerArgs args);

        public static readonly BuddyList Instance = new BuddyList();
        private bool notificationDelayed;
        private readonly HashSet<Presence> PresenceReserve = new HashSet<Presence>();

        private BuddyList() : base(BuddyComparer.Instance)
        {
        }

        public void AddRange(IEnumerable<Buddy> items)
        {
            DelayNotification(true);

            var buddies = items.Distinct(BuddyComparer.Instance).ToList();

            foreach (var buddy in buddies)
                Add(buddy);
            
            DelayNotification(false);

            if (!notificationDelayed && BuddyListChanged != null)
            {
                BuddyListChanged.Invoke(this,
                    new BuddyListChangedEventHandlerArgs(BuddyListChangedAction.Add, buddies.ToList()));
            }
        }

        private void DelayNotification(bool delay)
        {
            notificationDelayed = delay;
        }

        private new void Add(Buddy item)
        {
            base.Add(item);

            if (!notificationDelayed && BuddyListChanged != null)
            {
                BuddyListChanged.Invoke(this,
                    new BuddyListChangedEventHandlerArgs(BuddyListChangedAction.Add, new[] { item }));
            }
        }

        public void Add(RosterItem item)
        {
            var presences = PresenceReserve.Where(p => p.From.Bare.ToLower() == item.Jid.Bare.ToLower());
            Add(new Buddy(presences, item));
        }

        public void HandlePresence(Presence presence)
        {
            if (presence.Type == PresenceType.unavailable)
            {
                Remove(presence);
            }
            else
            {
                if (Contains(presence.From))
                {
                    Update(presence);
                }
                else
                {
                    PresenceReserve.Add(presence);
                }
            }
        }

        public void Update(Presence presence)
        {
            var buddy = Get(presence.From);
            
            if (buddy == null)
                return;

            buddy.UpdateInfo(presence);

            if (!notificationDelayed && BuddyListChanged != null)
            {
                BuddyListChanged.Invoke(this,
                    new BuddyListChangedEventHandlerArgs(BuddyListChangedAction.Update, new[] { buddy }));
            }
        }

        public bool Contains(Jid jid)
        {
            return Get(jid) != null;
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

            if (buddy.IsOnline)
            {
                BuddyListChanged.Invoke(this,
                    new BuddyListChangedEventHandlerArgs(BuddyListChangedAction.Update, new[] { buddy }));
            }
            else
            {
                Remove(buddy);
                if (notificationDelayed || BuddyListChanged != null) return;
                BuddyListChanged.Invoke(this,
                    new BuddyListChangedEventHandlerArgs(BuddyListChangedAction.Remove, new[] { buddy }));
            }
        }

        public new void Remove(Buddy item)
        {
            base.Remove(item);
            if (!notificationDelayed && BuddyListChanged != null)
            {
                BuddyListChanged.Invoke(this,
                    new BuddyListChangedEventHandlerArgs(BuddyListChangedAction.Remove, new[] {item}));
            }
        }

        public void UpdateRange(IList<Presence> updateBuddies)
        {
            DelayNotification(true);

            foreach (var presence in updateBuddies)
                Update(presence);
            
            BuddyListChanged.Invoke(this,
                new BuddyListChangedEventHandlerArgs(BuddyListChangedAction.Update, updateBuddies.Select(p => Get(p.From)).ToList()));
            DelayNotification(false);
        }

        public event BuddyListChangedEventHandler BuddyListChanged;
    }
}
