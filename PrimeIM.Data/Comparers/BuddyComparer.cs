using System;
using System.Collections.Generic;
using agsXMPP.protocol.client;

namespace PrimeIM.Data.Comparers
{
    public class BuddyComparer : IComparer<Buddy>, IEqualityComparer<Buddy>
    {
        public static readonly BuddyComparer Instance = new BuddyComparer();
        private BuddyComparer(){}
        
        public int Compare(Buddy x, Buddy y)
        {
            if (x.Jid.Bare.ToLower() == y.Jid.Bare.ToLower())
                return 0;

            int xInt = GetShowTypeInt(x);
            int yInt = GetShowTypeInt(y);
            
            if (xInt > yInt)
                return 1; // greater than
            if (xInt < yInt)
                return -1; // less than
            if (x.Name == y.Name)
                return 1; // if nicknames are the same need to return > so the hashset allows adding
            return x.Name.CompareTo(y.Name);

        }

        private int GetShowTypeInt(Buddy buddy)
        {
            if (buddy.MainPresence == null)
                return 5;

            switch (buddy.MainPresenceType)
            {
                case ShowType.chat:
                    return 0;
                case ShowType.NONE:
                    return 1;
                case ShowType.away:
                    return 2;
                case ShowType.xa:
                    return 3;
                case ShowType.dnd:
                    return 4;
                default:
                    return 5;
            }

        }

        #region Implementation of IEqualityComparer<in Buddy>

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        /// <param name="x">The first object of type <paramref name="T"/> to compare.</param><param name="y">The second object of type <paramref name="T"/> to compare.</param>
        public bool Equals(Buddy x, Buddy y)
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
        public int GetHashCode(Buddy obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
}
