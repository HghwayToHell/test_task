using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Words
{
    public class Counter
    {
        private static char[] _separators = { ' ', ',', '.' ,'\n','-' ,'\t','–' ,' ' , '…', '(', ')', '[', ']',
                                              '1', '2' ,'3' ,'4' ,'5' ,'6' ,'7' ,'8' ,'9' ,'0', ';', ':' };

        public static Dictionary<string, int> Count(StreamReader input)
        {
            Dictionary<string, int> wordsCount = new Dictionary<string, int>();
            string? line;
            while ((line = input.ReadLine()) != null)
            {
                IEnumerable<string> words = LazySplitter.Split(line.ToLower(), _separators);
                foreach (var word in words)
                {
                    if (wordsCount.ContainsKey(word))
                    {
                        wordsCount[word]++;
                    }
                    else
                    {
                        wordsCount.Add(word, 1);
                    }
                }
            }

            return wordsCount;
        }
    }
}
