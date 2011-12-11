using System;
using System.Collections.Generic;
using System.Diagnostics;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.extensions.chatstates;
using agsXMPP.protocol.iq.avatar;
using agsXMPP.protocol.iq.roster;
using PrimeIM.Data.Comparers;
using System.Linq;
using Utility.Concretes;
using Utility.Interfaces;

namespace PrimeIM.Data
{
    [DebuggerDisplay("{Username}")]
    public class Buddy : BindableObject, IComparable<Buddy>
    {
        private readonly SortedSet<Presence> presences = new SortedSet<Presence>(PresenceComparer.Instance);
        public ISet<Presence> Presences
        {
            get { return presences;} }

        private Presence activePresence;

        public List<string> Resources
        {
            get { return presences.Select(p => p.From.Resource).ToList(); }
        }
        public Jid Jid { get { return MainPresence.From; } }
        
        public bool IsOnline
        {
            get { return presences.Count > 0; }
        }

        public string Email
        {
            get { return MainPresence.From.Bare.ToLower(); }
        }

        public RosterItem RosterItem { get; private set; }
        
        public string Name
        {
            get { return RosterItem == null ? Username : RosterItem.Name; }
        }

        public string Username
        {
            get { return MainPresence.From.User; }
        }
        
        public string Status
        {
            get { return MainPresence.Status; }
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

        //public Buddy(Presence presence)
        //{
        //    UpdateInfo(presence);
        //}

        public Buddy(IEnumerable<Presence> presences, RosterItem item)
        {
            this.presences = new SortedSet<Presence>(presences.Distinct(PresenceComparer.Instance), PresenceComparer.Instance);
            this.RosterItem = item;
        }

        public void UpdateInfo(Presence presence)
        {
            activePresence = presences.Where(p => p.From.Resource == presence.From.Resource).SingleOrDefault();
            
            if (activePresence != null)
                RemovePresence(activePresence);
            
            presences.Add(presence);
        }

        public void SendChatState(Chatstate state, string resource, string thread)
        {
            PimMessageHandler.SendChatState(state, this, resource, thread);
        }


        public void RemovePresence(Presence presence)
        {
            presences.RemoveWhere(p => p.From.Resource.Equals(presence.From.Resource));
        }

        #region Equality Members

        public bool Equals(Buddy other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Email, Email);
        }

        public bool Equals(Presence other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return presences.Contains(other);
        }

        public int CompareTo(Buddy other)
        {
            return BuddyComparer.Instance.Compare(this, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            
            if (obj.GetType() == typeof(Buddy))
                return Equals(obj as Buddy);
            if (obj.GetType() == typeof(Presence))
                return Equals(obj as Presence); 

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Email != null ? Email.GetHashCode() : 0) * 397);
            }
        }

        #endregion

    }
}
