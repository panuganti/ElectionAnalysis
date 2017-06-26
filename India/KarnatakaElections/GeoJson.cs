using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KarnatakaElections
{
    public class GeoJsonUtils
    {
        public static GeoJson KeepKarnatakaOnly(string inGeoJsonFile, string outGeoJsonFile, string state)
        {
            var geoJson = JsonConvert.DeserializeObject<GeoJson>(File.ReadAllText(inGeoJsonFile));
            var outGeoJson = geoJson;
            outGeoJson.features = geoJson.features.Where(f => f.properties.state.Equals(state)).ToArray();
            return outGeoJson;
        }

        public static void PrintNamesAndIds(GeoJson geoJson, string outfile)
        {
            File.WriteAllLines(outfile, geoJson.features.Select(f => String.Join("\t", f.properties.state, f.properties.ac.ToString(), f.properties.ac_name, f.properties.pc.ToString(), f.properties.pc_name)));
        }
    }

    public class GeoJson
    {
        public string type { get; set; }
        public Crs crs { get; set; }
        public Feature[] features { get; set; }
    }

    public class Crs
    {
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
    }

    public class Feature
    {
        public string type { get; set; }
        public Properties1 properties { get; set; }
        public Geometry geometry { get; set; }
    }

    public class Properties1
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
        public float[][][] coordinates { get; set; }
    }

}
