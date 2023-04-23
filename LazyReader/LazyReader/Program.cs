using System;
using System.IO;

namespace Words
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2) 
            {
                Console.WriteLine("Usage: LazyReader.exe input.txt output.txt");
                return;
            }
            
            Dictionary<string, int> countedDict;


            using (StreamReader sr = new StreamReader(args[0]))
            {
                countedDict = Counter.Count(sr);

            }

            var sortedWordsCount = countedDict.OrderByDescending(x => x.Value);

            using (StreamWriter sw = new StreamWriter(args[1]))
            {
                foreach (var item in sortedWordsCount)
                {
                    sw.WriteLine(item);
                }
            }
        }
    }
}
