using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PrimeIM.Data.Comparers;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.iq.avatar;
using agsXMPP.protocol.iq.roster;

namespace PrimeIM.Data
{
    [DebuggerDisplay("{Username}")]
    public class Buddy : IComparable<Buddy>
    {
        private readonly SortedSet<Presence> presences;

        private Presence activePresence;

        public Buddy(IEnumerable<Presence> presences, RosterItem item)
        {
            this.presences = new SortedSet<Presence>(presences, PresenceComparer.Instance);
            RosterItem = item;
        }

        public List<string> Resources
        {
            get { return presences.Select(p => p.From.Resource).ToList(); }
        }

        public Jid Jid
        {
            get { return RosterItem.Jid; }
        }

        public bool IsOnline
        {
            get { return presences != null && presences.Count > 0; }
        }

        public string Email
        {
            get { return Jid.Bare.ToLower(); }
        }

        public RosterItem RosterItem { get; private set; }

        public string Name
        {
            get { return RosterItem == null ? Username : RosterItem.Name; }
        }

        public string Username
        {
            get { return Jid.User; }
        }

        public string Status
        {
            get { return MainPresence == null ? null : MainPresence.Status; }
        }

        public Presence MainPresence
        {
            get
            {
                return presences.Where(p => p.IsPrimary).FirstOrDefault() ?? presences.Min;
            }
        }

        public ShowType MainPresenceType
        {
            get { return MainPresence.Show; }
        }

        public string ShowTypeString
        {
            get
            {
                switch (MainPresenceType)
                {
                    case ShowType.chat:
                    case ShowType.NONE:
                        return @"Available";
                    case ShowType.away:
                        return @"Idle";
                    case ShowType.xa:
                        return @"Extended Idle";
                    case ShowType.dnd:
                        return @"Busy";
                    default:
                        return string.Empty;
                }
            }
        }

        public string LongName
        {
            get { return String.Format("{0} ({1})", Name, Email); }
        }

        public Avatar Avatar
        {
            get { return null; }
            set { value = null; }
        }

        #region IComparable<Buddy> Members

        public int CompareTo(Buddy other)
        {
            return BuddyComparer.Instance.Compare(this, other);
        }

        #endregion

        public void UpdateInfo(Presence presence)
        {
            activePresence = presences.Where(p => p.From.Resource == presence.From.Resource).SingleOrDefault();

            if (activePresence != null)
                RemovePresence(activePresence);

            presences.Add(presence);
        }

        public void RemovePresence(Presence presence)
        {
            presences.RemoveWhere(p => p.From.Resource.Equals(presence.From.Resource));
        }

        public bool Equals(Buddy other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Email, Email);
        }

        public bool Equals(Jid other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Jid.Bare.ToLower().Equals(other.Bare.ToLower());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (obj.GetType() == typeof (Buddy))
                return Equals(obj as Buddy);
            if (obj.GetType() == typeof (Jid))
                return Equals(obj as Jid);

            throw new Exception(string.Format("Cannot compare {0} to Buddy.", obj.GetType().Name));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Email != null ? Email.GetHashCode() : 0)*397);
            }
        }
    }
}