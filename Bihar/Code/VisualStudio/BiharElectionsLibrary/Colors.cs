using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BiharElectionsLibrary
{
    [DataContract]
    public class Colors
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Color { get; set; }

        private static List<Colors> _colors {get; set;}
        public static string GetColor(string name)
        {
            if (_colors == null) { LoadColors();}
            return _colors.First(t => t.Name.ToLower() == name.ToLower()).Color;
        }

        public static List<Colors> LoadColors()
        {
            var lines = File.ReadAllLines("./Legend.txt");
            _colors =  lines.Select(t => 
            {
                var parts = t.Split('\t'); return new Colors { Name = parts[0], Color = parts[1] };
            }).ToList();
            return _colors;
        }
    }
}
