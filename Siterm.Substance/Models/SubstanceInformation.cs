using System.Collections.Generic;

namespace Siterm.Substance.Models
{
    public class SubstanceInformation
    {
        public List<PSentence> PSentences { get; set; }
        public List<HSentence> HSentences { get; set; }
        public Dictionary<string, string> AdditionalInfos { get; set; }
        public string[] AlternativeNames { get; set; }
        public List<string> IconPaths { get; set; }
    }
}