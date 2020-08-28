using System.Collections.Generic;
using Siterm.Excel.Services;
using Siterm.Substance.Models;

namespace Siterm.Substance.Services
{
    public class HSentenceFactory
    {
        private readonly Dictionary<string, string> _hSentenceDictionary;
        private readonly Dictionary<string, string> _hIconDictionary;

        public HSentenceFactory(ReadHSentencesService readHSentencesService)
        {
            _hSentenceDictionary = readHSentencesService.GetHSentences();
            _hIconDictionary = readHSentencesService.GetHIcon();
        }

        public HSentence GetHSentence(string name)
        {
            return !_hSentenceDictionary.ContainsKey(name)
                ? null
                : new HSentence(name, _hSentenceDictionary[name], ComposeIconPath(_hIconDictionary[name]));
        }

        private static string ComposeIconPath(string iconName)
        {
            return !string.IsNullOrEmpty(iconName)
                ? $"pack://application:,,,/Siterm.WPF;component/Resources/Images/Icons/{iconName}.png"
                : null;
        }
    }
}