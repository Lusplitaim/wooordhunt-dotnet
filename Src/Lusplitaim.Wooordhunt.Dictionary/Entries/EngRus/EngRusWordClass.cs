
using System.Collections.Generic;

namespace Lusplitaim.Wooordhunt
{
    public class EngRusWordClass
    {
        public string PartOfSpeech { get; }
        public IEnumerable<EngRusTranslation>? Translations { get; set; }

        public EngRusWordClass(string partOfSpeech)
        {
            PartOfSpeech = partOfSpeech;
        }
    }
}
