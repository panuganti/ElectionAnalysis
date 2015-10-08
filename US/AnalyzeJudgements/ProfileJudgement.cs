using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeJudgements
{
    public class ProfileJudgement
    {
        public string Judge { get; set; }
        public string Profile { get; set; }
        public string PicJudgement { get; set; }
        public string OverallJudgement { get; set; }
        public List<string> TweetJudgements { get; set; }
    }
}
