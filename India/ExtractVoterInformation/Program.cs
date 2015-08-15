using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ExtractVoterInformation
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = @"C:\Users\rapanuga\Desktop\ElectoralRoll_1_1.txt";
            var lines = File.ReadAllLines(filename, Encoding.Unicode);
            Console.Write(lines);
        }
    }
}
