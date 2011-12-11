using System.Collections.Generic;
using agsXMPP.protocol.client;

namespace PrimeIM.Data.Comparers
{
    public class ShowTypeComparer : IComparer<ShowType>
    {
        public static readonly ShowTypeComparer Singleton = new ShowTypeComparer();
        private ShowTypeComparer()
        {
            
        }
        public int Compare(ShowType x, ShowType y)
        {
            byte xByte = ShowTypeToByte(x);
            byte yByte = ShowTypeToByte(y);

            if (xByte > yByte)
                return 1; // greater than
            if (xByte < yByte)
                return -1; // less than

            return 0; // equal

        }

        private byte ShowTypeToByte(ShowType showType)
        {
            switch (showType)
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

    }
}