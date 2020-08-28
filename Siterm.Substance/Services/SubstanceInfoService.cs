using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Siterm.Substance.Models;

namespace Siterm.Substance.Services
{
    public class SubstanceInfoService
    {
        private const string MetaFileName = "meta.xml";
        private readonly HSentenceFactory _hSentenceFactory;
        private readonly PSentenceFactory _pSentenceFactory;

        public SubstanceInfoService(PSentenceFactory pSentenceFactory, HSentenceFactory hSentenceFactory)
        {
            _pSentenceFactory = pSentenceFactory;
            _hSentenceFactory = hSentenceFactory;
        }

        public XDocument MetaFile { get; set; }

        public SubstanceInformation GetSubstanceInformation(Domain.Models.Substance substance)
        {
            var substanceInformation = new SubstanceInformation();
            var metaFilePath = Path.Combine(substance.Path, MetaFileName);

            GetMetaFile(metaFilePath);
            if (MetaFile is null) return substanceInformation;

            SetPSentences(substanceInformation);
            SetHSentences(substanceInformation);
            SetAdditionalInfos(substanceInformation);
            SetAlternativeNames(substanceInformation);
            SetIconPaths(substanceInformation);

            return substanceInformation;
        }

        private void SetPSentences(SubstanceInformation substanceInformation)
        {
            substanceInformation.PSentences = MetaFile.Descendants("p").Descendants("item")
                .Select(element => _pSentenceFactory.GetPSentence(element.Value))
                .Where(p => p != null).ToList();
        }

        private void SetHSentences(SubstanceInformation substanceInformation)
        {
            substanceInformation.HSentences = MetaFile.Descendants("h").Descendants("item")
                .Select(element => _hSentenceFactory.GetHSentence(element.Value))
                .Where(h => h != null).ToList();
        }

        private void SetAdditionalInfos(SubstanceInformation substanceInformation)
        {
            substanceInformation.AdditionalInfos = MetaFile.Descendants("others").Descendants()
                .GroupBy(e => e.Name.LocalName)
                .Select(g => g.First())
                .ToDictionary(element => element.Name.LocalName, element => element.Value);
        }

        private void SetAlternativeNames(SubstanceInformation substanceInformation)
        {
            substanceInformation.AlternativeNames = MetaFile.Descendants("otherNames").Descendants("name")
                .Select(element => element.Value).ToArray();
        }

        private static void SetIconPaths(SubstanceInformation substanceInformation)
        {
            var icons = new List<string>();
            foreach (var hSentence in substanceInformation.HSentences.Where(hSentence =>
                !icons.Contains(hSentence.IconPath))) icons.Add(hSentence.IconPath);

            substanceInformation.IconPaths = icons;
        }

        public void GetMetaFile(string path)
        {
            if (File.Exists(path))
                MetaFile = XDocument.Load(path);
        }
    }
}