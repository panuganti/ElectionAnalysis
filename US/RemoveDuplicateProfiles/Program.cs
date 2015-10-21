using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveDuplicateProfiles
{
    class Program
    {
        static void Main(string[] args)
        {
            File.WriteAllLines(@"C:\Projects\ElectionAnalysis\US\Judging\allHandlesDistinct.txt", File.ReadAllLines(@"C:\Projects\ElectionAnalysis\US\Judging\allHandles.txt").Distinct());
        }
    }
}
