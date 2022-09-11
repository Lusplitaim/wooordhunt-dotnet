
using System.Collections.Generic;

namespace Lusplitaim.Wooordhunt
{
    public class EngEntryContent
    {
        public IEnumerable<EngWordClass>? WordClasses { get; set; }
        public IEnumerable<TranscriptionSet>? Transcriptions { get; set; }
    }
}
