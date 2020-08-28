using System.Collections.Generic;
using Siterm.Excel.Services;
using Siterm.Substance.Models;

namespace Siterm.Substance.Services
{
    public class PSentenceFactory
    {
        private readonly Dictionary<string, string> _pSentenceDictionary;

        public PSentenceFactory(ReadPSentencesService readPSentencesService)
        {
            _pSentenceDictionary = readPSentencesService.GetPSentences();
        }

        public PSentence GetPSentence(string name)
        {
            return !_pSentenceDictionary.ContainsKey(name) ? null : new PSentence(name, _pSentenceDictionary[name]);
        }
    }
}