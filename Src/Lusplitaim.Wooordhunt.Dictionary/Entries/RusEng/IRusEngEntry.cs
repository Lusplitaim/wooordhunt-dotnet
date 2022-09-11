
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    public interface IRusEngEntry
    {
        public Task<RusEngEntryContent> GetContent();
        public Task<IEnumerable<Equivalent>?> GetEquivalents();
        public Task<IEnumerable<Phrase>?> GetCollocations();
        public Task<IEnumerable<Equivalent>?> GetRelatedWords();
        public Task<IEnumerable<Phrase>?> GetSentences();
    }
}
