
namespace Lusplitaim.Wooordhunt
{
    public class Phrase
    {
        public string Original { get; set; }
        public string Translation { get; set; }

        public Phrase(string original, string translation)
        {
            Original = original;
            Translation = translation;
        }
    }
}
