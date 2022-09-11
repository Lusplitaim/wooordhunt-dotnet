
using System.Collections.Generic;

namespace Lusplitaim.Wooordhunt
{
    public class EngRusEntryContent
    {
        public IEnumerable<EngRusWordClass>? WordClasses { get; set; }
        public IEnumerable<TranscriptionSet>? Transcriptions { get; set; }
        public IEnumerable<Phrase>? Collocations { get; set; }
        public IEnumerable<Phrase>? Sentences { get; set; }
        public IEnumerable<Phrase>? SimilarWords { get; set; }
        public IEnumerable<Phrase>? PhrasalVerbs { get; set; }
        public IEnumerable<WordForm>? WordForms { get; set; }
    }
}
