using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Words
{
    public class LazySplitter
    {
        public static IEnumerable<string> Split(string toSplit, char[] separators)
        {
            if (string.IsNullOrEmpty(toSplit))
            {
                yield break;
            }

            var sb = new StringBuilder();

            foreach (char c in toSplit)
            {
                if (separators.Contains(c))
                {
                    if (sb.Length > 0)
                    {
                        yield return sb.ToString();
                    }
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }

            if (sb.Length > 0)
            {
                yield return sb.ToString();
            }
        }

    }
}
