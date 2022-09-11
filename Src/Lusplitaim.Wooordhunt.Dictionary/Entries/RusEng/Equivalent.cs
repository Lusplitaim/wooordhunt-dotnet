
using System.Collections.Generic;

namespace Lusplitaim.Wooordhunt
{
    public class Equivalent
    {
        public string EnglishWord { get; set; }
        public string? Transcription { get; set; }
        public string Translation { get; set; }
        public IEnumerable<Phrase>? Examples { get; set; }
    }
}
