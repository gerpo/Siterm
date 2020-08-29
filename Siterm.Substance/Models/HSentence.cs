namespace Siterm.Substance.Models
{
    public class HSentence : ISentence
    {
        public HSentence(string name, string sentence, string iconPath)
        {
            Name = name;
            Sentence = sentence;
            IconPath = iconPath;
        }

        public string IconPath { get; }
        public string Name { get; }
        public string Sentence { get; }
    }
}