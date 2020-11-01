using System;
using System.Linq;

namespace Labyrinths.Utils
{
    public static class Extensions
    {
        public static string Repeat(this char c, int count)
        {
            return new string(c, count);
        }
    }
}
