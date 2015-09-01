using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ElectionDataContracts
{
    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PoliticalParty
    {
        // ReSharper disable InconsistentNaming
        bjp,
        rslp,
        jdu,
        rjd,
        ljp,
        inc,
        ind,
        cpi,
        sp,
        bsp,
        ncp,
        aap,
        jmm,
        ld,

        // Alliances
        nda,
        upa,

        // Misc
        others,
        cantSay = 99,
        didNotVote,
        missingVote,
        nota = 0,
        Error
        // ReSharper restore InconsistentNaming
    }
}
