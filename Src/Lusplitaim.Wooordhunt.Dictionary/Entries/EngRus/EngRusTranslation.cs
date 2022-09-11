
using System.Collections.Generic;

namespace Lusplitaim.Wooordhunt
{
    public class EngRusTranslation
    {
        public string Equivalent { get; set; }
        public IEnumerable<Phrase>? Examples { get; set; }

        public EngRusTranslation(string equivalent)
        {
            Equivalent = equivalent;
            Examples = new List<Phrase>();
        }
    }
}
