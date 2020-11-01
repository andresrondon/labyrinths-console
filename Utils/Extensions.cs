using System;
using System.Linq;

namespace Labyrinths.Utils
{
    public static class Extensions
    {
        public static string ToProper(this String s)
        {
            s = s.ToLower();
            var sFirstChar = s.ElementAt(0).ToString().ToUpper();
            s = s.Substring(1);
            return sFirstChar + s;
        }
        public static string Repeat(this char c, int count)
        {
            return new String(c, count);
        }
    }
}
