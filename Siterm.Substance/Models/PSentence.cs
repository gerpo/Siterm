namespace Siterm.Substance.Models
{
    public class PSentence : ISentence
    {
        public PSentence(string name, string sentence)
        {
            Name = name;
            Sentence = sentence;
        }

        public string Name { get; }
        public string Sentence { get; }
    }
}