using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace KarnatakaElections.GeoJson
{
    public class GeoJsonUtils
    {
        public static void PrintIdName(string infile, string outfile)
        {
            var geoJson = JsonConvert.DeserializeObject<GeoJson2>(File.ReadAllText(infile));
            Console.WriteLine(geoJson.features.Count());
            File.WriteAllText(outfile, string.Join("\n",geoJson.features.OrderBy(f => f.properties.ac).Select(f => String.Format("{0}\t{1}\t{2}\t{3}",f.properties.ac, f.properties.ac_name, f.properties.pc, f.properties.pc_name))));
        }
    }


    public class GeoJson2
    {
        public string type { get; set; }
        public Feature[] features { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Properties properties { get; set; }
        public Geometry geometry { get; set; }
    }

    public class Properties
    {
        public string state { get; set; }
        public int pc { get; set; }
        public string pc_name { get; set; }
        public int ac { get; set; }
        public string ac_name { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public object[][][] coordinates { get; set; }
    }

}
