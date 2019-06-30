using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Data
{
    static class Utils
    {
        public static T Choice<T>(this Random random, IList<T> list)
        {
            return list[random.Next(list.Count)];
        }

        public static string NextString(this Random random, int len = 10)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
                sb.Append(random.Next(127));
            return sb.ToString();
        }
    }
}
