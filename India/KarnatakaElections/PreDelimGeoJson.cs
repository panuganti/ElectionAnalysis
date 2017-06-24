using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnatakaElections
{    
    public class GeoJsonUtils
    {
        public static void KeepKarnatakaOnlyGeoJson(string infile, string outfile)
        {
            var allStatesGeoJson = JsonConvert.DeserializeObject<PreDelimGeoJson>(File.ReadAllText(infile));
            var karGeoJson = allStatesGeoJson;
            karGeoJson.features = allStatesGeoJson.features.Where(f => f.properties.ST_CODE.Equals("S10")).ToArray();
            File.WriteAllText(outfile, JsonConvert.SerializeObject(karGeoJson, Formatting.Indented));
        }

        public static void PrintIdName(string infile, string outfile)
        {
            var geoJson = JsonConvert.DeserializeObject<PreDelimGeoJson>(File.ReadAllText(infile));
            File.WriteAllText(outfile, string.Join("\n", geoJson.features.OrderBy(f=> f.properties.AC_NO)
                .Select(f => String.Format("{0}\t{1}\t{2}\t{3}", f.properties.AC_NO, f.properties.AC_NAME, f.properties.PC_NO, f.properties.PC_NAME))));
        }

    }

    public class PreDelimGeoJson
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
        public string DIST_NAME { get; set; }
        public int Assem_No { get; set; }
        public float AREA { get; set; }
        public float PERIMETER { get; set; }
        public int INDIAASSEM { get; set; }
        public int INDIAASS { get; set; }
        public int NO { get; set; }
        public string ST_CODE { get; set; }
        public string AC_NAME { get; set; }
        public string AC_TYPE { get; set; }
        public int PC_NO { get; set; }
        public int AC_NO { get; set; }
        public string AC_HNAME { get; set; }
        public string PARTY { get; set; }
        public string CODE_NO { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string State { get; set; }
        public string PC_NAME { get; set; }
        public string PC_TYPE { get; set; }
        public string PC_CODE { get; set; }
        public int PC_NO_1 { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }
        public object[][][] coordinates { get; set; }
    }

}
