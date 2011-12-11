using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP.protocol.iq.roster;

namespace PrimeIM.Data.Comparers
{
    class RosterItemComparer : IComparer<RosterItem>, IEqualityComparer<RosterItem>
    {
        public static RosterItemComparer Instance = new RosterItemComparer();
        private RosterItemComparer()
        {
        }

        #region Implementation of IComparer<in Presence>

        public int Compare(RosterItem x, RosterItem y)
        {
            return x.Jid.Bare.ToLower().CompareTo(y.Jid.Bare.ToLower());
        }

        #endregion

        #region Implementation of IEqualityComparer<in Presence>

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.</param><param name="y">The second object of type <paramref name="T"/> to compare.</param>
        public bool Equals(RosterItem x, RosterItem y)
        {
            return Compare(x, y) == 0;
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <returns>
        /// A hash code for the specified object.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param><exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(RosterItem obj)
        {
            unchecked
            {
                return ((obj.Jid.Bare != null ? obj.Jid.Bare.ToLower().GetHashCode() : 0) * 397);
            }
        }

        #endregion
    }
}
