using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using agsXMPP.protocol.client;

namespace PrimeIM.Data.Comparers
{
    public class PresenceComparer : IComparer<Presence>, IEqualityComparer<Presence>
    {
        public static PresenceComparer Instance = new PresenceComparer();
        private PresenceComparer()
        {
        }

        #region Implementation of IComparer<in Presence>

        public int Compare(Presence x, Presence y)
        {
            return x.From.Resource.ToLower().CompareTo(y.From.Resource.ToLower());
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
        public bool Equals(Presence x, Presence y)
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
        public int GetHashCode(Presence obj)
        {
            unchecked
            {
                return ((obj.From.Resource != null ? obj.From.Resource.GetHashCode() : 0)*397);
            }
        }

        #endregion
    }
}