
using System.Collections.Generic;

namespace Lusplitaim.Wooordhunt
{
    public class RusEngEntryContent
    {
        public IEnumerable<Equivalent>? Equivalents { get; set; }
        public IEnumerable<Phrase>? Collocations { get; set; }
        public IEnumerable<Equivalent>? RelatedWords { get; set; }
        public IEnumerable<Phrase>? Sentences { get; set; }
    }
}
