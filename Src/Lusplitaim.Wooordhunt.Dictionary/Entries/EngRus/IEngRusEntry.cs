
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    public interface IEngRusEntry
    {
        public Task<EngRusEntryContent> GetContent();
        public Task<IEnumerable<TranscriptionSet>?> GetTranscriptions();
        public Task<IEnumerable<EngRusWordClass>?> GetWordClasses();
        public Task<IEnumerable<Phrase>?> GetCollocations();
        public Task<IEnumerable<Phrase>?> GetSentences();
        public Task<IEnumerable<Phrase>?> GetSimilarWords();
        public Task<IEnumerable<Phrase>?> GetPhrasalVerbs();
        public Task<IEnumerable<WordForm>?> GetWordForms();
    }
}
